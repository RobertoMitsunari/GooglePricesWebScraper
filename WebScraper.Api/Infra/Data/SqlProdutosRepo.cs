using System;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Common.Model;

namespace WebScraper.Api.Infra.Data
{
    public class SqlProdutosRepo : IProdutoRepo
    {
        private readonly ProdutoContext _context;

        public SqlProdutosRepo(ProdutoContext context)
        {
            _context = context;
        }

        public void CreateProduto(Produto produto)
        {
            if (produto is null)
            {
                throw new ArgumentNullException(nameof(Produto));
            }

            _context.Produtos.Add(produto);
        }

        public void DeleteProduto(Produto produto)
        {
            if (produto is null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Remove(produto);
        }

        public void DeleteProdutos(IEnumerable<Produto> produtos)
        {
            if (produtos is null)
            {
                throw new ArgumentNullException(nameof(produtos));
            }

            _context.RemoveRange(produtos);
        }

        public Produto GetProdutoById(int id)
        {
            return _context.Produtos.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Produto> GetProdutos()
        {
            return _context.Produtos.ToList();
        }

        public IEnumerable<Produto> GetProdutosByName(string nome)
        {
            return _context.Produtos.Where(x => x.Nome == nome);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateProduto(Produto Produto)
        {
            // No SQL agt n precisa implementar nada no update mas esse método é adicionado caso alguma outra forma
            // de persistencia acabe surgindo no projeto, nesse caso esse método vai ser utilizado.
        }
    }
}
