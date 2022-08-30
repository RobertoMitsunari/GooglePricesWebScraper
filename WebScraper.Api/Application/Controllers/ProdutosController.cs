using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebScraper.Api.Application;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Common.Model;

namespace WebScraper.Api.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepo _repo;
        private readonly ColetorRunner coletor;

        public ProdutosController(IProdutoRepo repo, IPesquisaFacade pesquisaFacade)
        {
            _repo = repo;
            coletor = new ColetorRunner(pesquisaFacade);
        }

        [HttpGet("DB/CollectAndStore/{produtoNome}")]
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
