using System.Collections.Generic;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Common.Domain.Model;

namespace WebScraper.Api.Application.Facade
{
    public class PesquisaFacade : IPesquisaFacade
    {
        private static Dictionary<string, Pesquisa> _pesquisasDictionary;

        public PesquisaFacade()
        {
            _pesquisasDictionary = new Dictionary<string, Pesquisa>();
        }

        public bool VerifyIfSearchIsNeccesary(string produtoNome, out Pesquisa pesquisa)
        {
            if (_pesquisasDictionary.TryGetValue(produtoNome, out Pesquisa oldPesquisa))
            {
                pesquisa = oldPesquisa;

                if (oldPesquisa.DataPesquisa.AddMinutes(10.0) < System.DateTime.Now)
                {
                    return true;
                }

                return false;
            }

            pesquisa = null;
            return true;
        }

        public void InsertOrUpdatePesquisa(Pesquisa pesquisa, string produto)
        {
            _pesquisasDictionary.Add(produto, pesquisa);
        }

        public void LoadPesquisasDictionary(List<Pesquisa> pesquisas)
        {
            pesquisas.ForEach(s => _pesquisasDictionary.Add(s.Name, s));
        }
    }
}
