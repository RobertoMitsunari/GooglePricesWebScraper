using System.Collections.Generic;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Domain.Contracts
{
    public interface IColetorRunner
    {
        public void CollectAndStore(string produtoNome);
        public void UpdateOrInsertPesquisa(Pesquisa pesquisa,List<Produto> produtos, string produto);
        public List<Produto> CollectProducts(string produtoNome);
        public void Start();
        public void Stop();
    }
}
