using System.Collections.Generic;
using WebScraper.Common.Domain.Model;

namespace WebScraper.Api.Domain.Contracts
{
    public interface IPesquisaFacade
    {
        public bool VerifyIfSearchIsNeccesary(string produtoNome, out Pesquisa pesquisa);

        public void LoadPesquisasDictionary(IEnumerable<Pesquisa> pesquisas);

        public void InsertOrUpdatePesquisa(Pesquisa pesquisa, string produto);

        public List<string> GetPesquisas();
    }
}
