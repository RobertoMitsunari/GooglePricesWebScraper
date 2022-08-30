using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Infra.Data;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Infra
{
    public class DatabaseHelper
    {
        private readonly DbContextOptionsBuilder<ProdutoContext> optionsBuilder;
        private readonly ProdutoContext context;
        private readonly SqlPesquisasRepo pesquisasRepo;
        private readonly SqlProdutosRepo produtosRepo;

        public DatabaseHelper()
        {
            optionsBuilder = new DbContextOptionsBuilder<ProdutoContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=ProdutosDB;User ID=sa;Password=Roberto@123;");

            context = new ProdutoContext(optionsBuilder.Options);
            pesquisasRepo = new SqlPesquisasRepo(context);
            produtosRepo = new SqlProdutosRepo(context);
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
            if (pesquisa is null)
            {
                pesquisasRepo.CreatePesquisa(pesquisa);
            }
            else
            {
                pesquisa.DataPesquisa = System.DateTime.Now;
            }

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
