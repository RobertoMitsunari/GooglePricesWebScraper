using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Common.Model;

namespace WebScraper.Api.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepo _repo;
        private readonly IColetorRunner coletor;

        public ProdutosController(IProdutoRepo repo, IColetorRunner coletorRunner)
        {
            _repo = repo;
            coletor = coletorRunner;
        }

        [HttpGet("CollectAndStore/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosAndStore(string produtoNome)
        {
            coletor.CollectAndStore(produtoNome);

            var produtos = _repo.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("DB/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosFromDB(string produtoNome)
        {

            var produtos = _repo.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetProdutos(string produtoNome)
        {
            var produtos = coletor.CollectProducts(produtoNome);

            return Ok(produtos);
        }
    }
}
