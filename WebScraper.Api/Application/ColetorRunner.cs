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

namespace WebScraper.Api.Application
{
    public class ColetorRunner : IColetorRunner
    {
        private List<Pesquisa> pesquisas;
        private readonly ScrapEngine engine;
        private readonly Cleaner cleaner;
        private readonly DatabaseHelper DatabaseHelper;
        private readonly IPesquisaFacade _pesquisaFacade;

        public ColetorRunner()
        {
            engine = new ScrapEngine();
            cleaner = new Cleaner();
            DatabaseHelper = new DatabaseHelper();
            _pesquisaFacade = new PesquisaFacade();
            LoadPesquisas();
        }

        public void CollectAndStore(string produtoNome)
        {
            if (!_pesquisaFacade.VerifyIfSearchIsNeccesary(produtoNome, out Pesquisa pesquisa))
            {
                return;
            }

            if (!(pesquisa is null))
            {
                DatabaseHelper.ClearOldSearch(produtoNome);
            }

            List<Produto> produtos = CollectProducts(produtoNome);

            DatabaseHelper.InsertProdutos(produtos);

            UpdateOrInsertPesquisa(pesquisa, produtos, produtoNome);
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
