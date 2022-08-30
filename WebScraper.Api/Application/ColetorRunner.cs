using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebScraper.Api.Infra;
using WebScraper.Application;
using WebScraper.Coletor.Application;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;
using System.Linq;
using WebScraper.Api.Domain.Contracts;

namespace WebScraper.Api.Application
{
    public class ColetorRunner
    {
        private List<Pesquisa> pesquisas;
        //private Dictionary<string, Pesquisa> pesquisasDictionary;
        private readonly ScrapEngine engine;
        private readonly Cleaner cleaner;
        private readonly DatabaseHelper DatabaseHelper;
        private readonly IPesquisaFacade _pesquisaFacade;

        public ColetorRunner(IPesquisaFacade pesquisaFacade)
        {
            engine = new ScrapEngine();
            cleaner = new Cleaner();
            DatabaseHelper = new DatabaseHelper();
            _pesquisaFacade = pesquisaFacade;
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

            DatabaseHelper.InsertProdutos(CollectProducts(produtoNome));

            UpdateOrInsertPesquisa(pesquisa, produtoNome);
        }

        public void UpdateOrInsertPesquisa(Pesquisa pesquisa, string produto)
        {
            if (pesquisa is null)
            {
                pesquisa = new Pesquisa(produto);
                DatabaseHelper.InsertPesquisa(pesquisa);
            }
            else
            {
                DatabaseHelper.UpdatePesquisa(pesquisa);
            }

            _pesquisaFacade.InsertOrUpdatePesquisa(pesquisa, produto);
        }

        public List<Produto> CollectProducts(string produtoNome)
        {
            var url = $"https://www.google.com.br/search?q=%22{produtoNome.Replace(" ", "+").Replace("_", "+")}%22&hl=pt-BR&tbm=shop";

            return cleaner.ClearData(engine.Coletar(url), produtoNome);
        }

        //public bool VerifyIfSearchIsNeccesary(string produtoNome, out Pesquisa pesquisa)
        //{
        //    if (pesquisasDictionary.TryGetValue(produtoNome, out Pesquisa oldPesquisa))
        //    {
        //        pesquisa = oldPesquisa;

        //        if (oldPesquisa.DataPesquisa.AddMinutes(10.0) < System.DateTime.Now)
        //        {
        //            DatabaseHelper.ClearOldSearch(produtoNome);

        //            return true;
        //        }

        //        return false;
        //    }

        //    pesquisa = null;
        //    return true;
        //}

        //private Dictionary<string, Pesquisa> InitializePesquisasDictionary(List<Pesquisa> pesquisas)
        //{
        //    var dictionary = new Dictionary<string, Pesquisa>();

        //    pesquisas.ForEach(s => dictionary.Add(s.Name, s));

        //    return dictionary;
        //}

        private void LoadPesquisas()
        {
            pesquisas = DatabaseHelper.GetPesquisas().ToList();
            _pesquisaFacade.LoadPesquisasDictionary(pesquisas);
        }
    }
}
