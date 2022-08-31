using System.Collections.Generic;
using WebScraper.Common.Domain.Model;

namespace WebScraper.Api.Domain.Contracts
{
    public interface IPesquisaRepo
    {
        bool SaveChanges();
        IEnumerable<Pesquisa> GetPesquisas();
        Pesquisa GetPesquisaById(int id);
        Pesquisa GetPesquisaByName(string name);
        void CreatePesquisa(Pesquisa pesquisa);
        void UpdatePesquisa(Pesquisa pesquisa);
        void DeletePesquisa(Pesquisa pesquisa);
    }
}
