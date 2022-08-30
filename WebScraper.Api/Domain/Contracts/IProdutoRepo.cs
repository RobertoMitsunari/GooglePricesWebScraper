using System.Collections.Generic;
using WebScraper.Common.Model;

namespace WebScraper.Api.Domain.Contracts
{
    public interface IProdutoRepo
    {
        bool SaveChanges();
        IEnumerable<Produto> GetProdutos();
        IEnumerable<Produto> GetProdutosByName(string name);
        Produto GetProdutoById(int id);
        void CreateProduto(Produto produto);
        void UpdateProduto(Produto produto);
        void DeleteProduto(Produto produto);
        void DeleteProdutos(IEnumerable<Produto> produtos);
    }
}
