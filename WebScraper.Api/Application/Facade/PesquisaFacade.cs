using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Infra.Data;
using WebScraper.Common.Domain.Model;

namespace WebScraper.Api.Application.Facade
{
    public class PesquisaFacade : IPesquisaFacade
    {
        public static Dictionary<string, Pesquisa> _pesquisasDictionary = new Dictionary<string, Pesquisa>();
        private readonly IPesquisaRepo pesquisasRepo;

        public PesquisaFacade(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProdutoContext>();

            pesquisasRepo = new SqlPesquisasRepo(context);

            LoadPesquisasDictionary(pesquisasRepo.GetPesquisas());
        }

        public bool VerifyIfSearchIsNeccesary(string produtoNome, out Pesquisa pesquisa)
        {
            if (_pesquisasDictionary.TryGetValue(produtoNome, out Pesquisa oldPesquisa))
            {
                pesquisa = oldPesquisa;

                if (oldPesquisa.DataPesquisa.AddMinutes(1.0) < System.DateTime.Now)
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
            if (_pesquisasDictionary.ContainsKey(produto))
            {
                _pesquisasDictionary[produto] = pesquisa;
            }
            else
            {
                _pesquisasDictionary.Add(produto, pesquisa);
            }
        }

        public void LoadPesquisasDictionary(IEnumerable<Pesquisa> pesquisas)
        {
            pesquisas.ToList().ForEach(s => _pesquisasDictionary.Add(s.Name, s));
        }

        public List<string> GetPesquisas()
        {
            var pesquisas = new List<string>();
            var keys = _pesquisasDictionary.Keys;

            foreach(string key in keys)
            {
                pesquisas.Add(key);
            }

            return pesquisas;
        }
    }
}
