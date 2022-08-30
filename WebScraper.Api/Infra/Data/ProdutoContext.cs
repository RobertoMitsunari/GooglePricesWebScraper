using Microsoft.EntityFrameworkCore;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Infra.Data
{
    public class ProdutoContext : DbContext
    {
        public ProdutoContext(DbContextOptions<ProdutoContext> opt) : base(opt) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pesquisa> Pesquisas { get; set; }
    }
}
