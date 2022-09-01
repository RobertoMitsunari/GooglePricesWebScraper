using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebScraper.Api.Infra;
using WebScraper.Application;
using WebScraper.Coletor.Application;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;
using System.Linq;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Application.Facade;
using System.Threading;

namespace WebScraper.Api.Application
{
    public class ColetorRunner : IColetorRunner
    {
        private List<Pesquisa> pesquisas;
        private bool runThread;
        private readonly ScrapEngine engine;
        private readonly Cleaner cleaner;
        private readonly DatabaseHelper DatabaseHelper;
        private readonly IPesquisaFacade _pesquisaFacade;
        private readonly Semaphore _semaphore;

        public ColetorRunner(IProdutoRepo produtoRepo, IPesquisaRepo pesquisaRepo)
        {
            engine = new ScrapEngine();
            cleaner = new Cleaner();
            DatabaseHelper = new DatabaseHelper(/*produtoRepo, pesquisaRepo*/);
            _pesquisaFacade = new PesquisaFacade();
            _semaphore = new Semaphore(initialCount: 1, maximumCount: 1);
            LoadPesquisas();
            Start();
        }

        public void Start()
        {
            runThread = true;

            var thread = new Thread(() =>
            {
                while (runThread)
                {
                    var searchKeys = _pesquisaFacade.GetPesquisas();
                    foreach (var key in searchKeys)
                    {
                        CollectAndStore(key);
                    }

                    Thread.Sleep(30 *   // minutes to sleep
                                 60 *   // seconds to a minute
                                 1000);
                }
            });
            thread.Start();
        }

        public void Stop()
        {
            runThread = false;
        }

        public void CollectAndStore(string produtoNome)
        {
            _semaphore.WaitOne();

            if (!_pesquisaFacade.VerifyIfSearchIsNeccesary(produtoNome, out Pesquisa pesquisa))
            {
                return;
            }

            List<Produto> produtos = CollectProducts(produtoNome);

            if (produtos.Count > 0)
            {
                if (!(pesquisa is null))
                {
                    DatabaseHelper.ClearOldSearch(produtoNome);
                }

                DatabaseHelper.InsertProdutos(produtos);

                UpdateOrInsertPesquisa(pesquisa, produtos, produtoNome);
            }

            _semaphore.Release();
        }

        public void UpdateOrInsertPesquisa(Pesquisa pesquisa, List<Produto> produtos, string produtoNome)
        {
            if (pesquisa is null)
            {
                pesquisa = new Pesquisa(produtoNome);
                pesquisa.CalculaValores(produtos);
                DatabaseHelper.InsertPesquisa(pesquisa);
            }
            else
            {
                pesquisa.CalculaValores(produtos);
                DatabaseHelper.UpdatePesquisa(pesquisa);
            }

            _pesquisaFacade.InsertOrUpdatePesquisa(pesquisa, produtoNome);
        }

        public List<Produto> CollectProducts(string produtoNome)
        {
            var url = $"https://www.google.com.br/search?q=%22{produtoNome.Replace(" ", "+").Replace("_", "+")}%22&hl=pt-BR&tbm=shop";

            return cleaner.ClearData(engine.Coletar(url), produtoNome);
        }

        //TODO Passar para o FACADE
        private void LoadPesquisas()
        {
            pesquisas = DatabaseHelper.GetPesquisas().ToList();
            _pesquisaFacade.LoadPesquisasDictionary(pesquisas);
        }
    }
}
