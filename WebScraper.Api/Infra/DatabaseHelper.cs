using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Infra.Data;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Infra
{
    public class DatabaseHelper
    {
        private readonly DbContextOptionsBuilder<ProdutoContext> optionsBuilder;
        private readonly ProdutoContext context;
        private readonly IPesquisaRepo pesquisasRepo;
        private readonly IProdutoRepo produtosRepo;

        public DatabaseHelper(/*IProdutoRepo produtoRepo, IPesquisaRepo pesquisaRepo*/)
        {
            optionsBuilder = new DbContextOptionsBuilder<ProdutoContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=ProdutosDB;User ID=sa;Password=Roberto@123;");

            context = new ProdutoContext(optionsBuilder.Options);
            pesquisasRepo = new SqlPesquisasRepo(context);
            produtosRepo = new SqlProdutosRepo(context);

            //produtosRepo = produtoRepo;
            //pesquisasRepo = pesquisaRepo;
        }

        public void InsertProdutos(IEnumerable<Produto> produtos)
        {
            foreach (Produto produto in produtos)
            {
                produtosRepo.CreateProduto(produto);
            }

            produtosRepo.SaveChanges();
        }

        public void InsertPesquisa(Pesquisa pesquisa)
        {
            pesquisasRepo.CreatePesquisa(pesquisa);

            pesquisasRepo.SaveChanges();
        }

        public void UpdatePesquisa(Pesquisa pesquisa)
        {
            pesquisa.DataPesquisa = System.DateTime.Now;

            pesquisasRepo.SaveChanges();
        }

        public void ClearOldSearch(string produtoNome)
        {
            var produtos = produtosRepo.GetProdutosByName(produtoNome).ToList().Where(s => s.DataPesquisa < System.DateTime.Now);
            //var produtos = produtosRepo.GetProdutosByName(produtoNome);

            produtosRepo.DeleteProdutos(produtos);

            pesquisasRepo.SaveChanges();
        }

        public IEnumerable<Pesquisa> GetPesquisas()
        {
            return pesquisasRepo.GetPesquisas();
        }
    }
}
