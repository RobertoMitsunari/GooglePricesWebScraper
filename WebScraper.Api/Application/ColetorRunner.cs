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
using Microsoft.Extensions.DependencyInjection;

namespace WebScraper.Api.Application
{
    public class ColetorRunner : IColetorRunner
    {
        private bool runThread;
        private readonly ScrapEngine _engine;
        private readonly DatabaseHelper _databaseHelper;
        private readonly IPesquisaFacade _pesquisaFacade;
        private readonly Semaphore _semaphore;

        public ColetorRunner(IServiceScopeFactory scopeFactory, IPesquisaFacade pesquisaFacade)
        {
            _engine = new ScrapEngine();
            _databaseHelper = new DatabaseHelper(scopeFactory);
            _pesquisaFacade = pesquisaFacade;
            _semaphore = new Semaphore(initialCount: 1, maximumCount: 1);

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

                    Thread.Sleep(5 *   // minutes to sleep
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
                    _databaseHelper.ClearOldSearch(produtoNome);
                }

                _databaseHelper.InsertProdutos(produtos);

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
                _databaseHelper.InsertPesquisa(pesquisa);
            }
            else
            {
                pesquisa.CalculaValores(produtos);
                _databaseHelper.UpdatePesquisa(pesquisa);
            }

            _pesquisaFacade.InsertOrUpdatePesquisa(pesquisa, produtoNome);
        }

        public void UpdateOrInsertPesquisa(Pesquisa pesquisa, string produtoNome)
        {
            if (pesquisa is null)
            {
                pesquisa = new Pesquisa(produtoNome);
                _databaseHelper.InsertPesquisa(pesquisa);
            }
            else
            {
                _databaseHelper.UpdatePesquisa(pesquisa);
            }

            _pesquisaFacade.InsertOrUpdatePesquisa(pesquisa, produtoNome);
        }

        public List<Produto> CollectProducts(string produtoNome)
        {
            return _engine.Coletar(produtoNome);
        }
    }
}
