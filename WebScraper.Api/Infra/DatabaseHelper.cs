using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IPesquisaRepo pesquisasRepo;
        private readonly IProdutoRepo produtosRepo;

        public DatabaseHelper(/*IProdutoRepo produtoRepo, IPesquisaRepo pesquisaRepo*/IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProdutoContext>();

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

        public Pesquisa GetPesquisaByName(string nome)
        {
            return pesquisasRepo.GetPesquisaByName(nome);
        }

        public IEnumerable<Produto> GetProdutosByName(string nome)
        {
            return produtosRepo.GetProdutosByName(nome);
        }

    }
}
