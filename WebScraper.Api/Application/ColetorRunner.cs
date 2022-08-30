using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebScraper.Api.Infra;
using WebScraper.Application;
using WebScraper.Coletor.Application;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;
using System.Linq;

namespace WebScraper.Api.Application
{
    public class ColetorRunner
    {
        private List<Pesquisa> pesquisas;
        private Dictionary<string, Pesquisa> pesquisasDictionary;
        private readonly ScrapEngine engine;
        private readonly Cleaner cleaner;
        private readonly DatabaseHelper DatabaseHelper;

        public ColetorRunner()
        {
            engine = new ScrapEngine();
            cleaner = new Cleaner();
            DatabaseHelper = new DatabaseHelper();

            LoadPesquisas();
        }

        public void CollectAndStore(string produtoNome)
        {
            Pesquisa pesquisa;
            if (!VerifyIfSearchIsNeccesary(produtoNome,out pesquisa))
            {
                return;
            }

            DatabaseHelper.InsertProdutos(CollectProducts(produtoNome));

            DatabaseHelper.InsertPesquisa(pesquisa);

            LoadPesquisas();
        }

        public List<Produto> CollectProducts(string produtoNome)
        {
            var url = $"https://www.google.com.br/search?q=%22{produtoNome.Replace(" ", "+").Replace("_", "+")}%22&hl=pt-BR&tbm=shop";

            return cleaner.ClearData(engine.Coletar(url), produtoNome);
        }

        public bool VerifyIfSearchIsNeccesary(string produtoNome, out Pesquisa pesquisa)
        {
            if (pesquisasDictionary.TryGetValue(produtoNome, out Pesquisa oldPesquisa))
            {
                pesquisa = oldPesquisa;

                if (oldPesquisa.DataPesquisa.AddMinutes(10.0) < System.DateTime.Now)
                {
                    DatabaseHelper.ClearOldSearch(produtoNome);

                    return true;
                }

                return false;
            }

            pesquisa = null;
            return true;
        }

        private Dictionary<string, Pesquisa> InitializePesquisasDictionary(List<Pesquisa> pesquisas)
        {
            var dictionary = new Dictionary<string, Pesquisa>();

            pesquisas.ForEach(s => dictionary.Add(s.Name, s));

            return dictionary;
        }

        private void LoadPesquisas()
        {
            pesquisas = DatabaseHelper.GetPesquisas().ToList();
            pesquisasDictionary = InitializePesquisasDictionary(pesquisas);
        }
    }
}
