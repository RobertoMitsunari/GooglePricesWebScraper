using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebScraper.Api.Application;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepo _produtosRepo;
        private readonly IPesquisaRepo _pesquisaRepo;
        private readonly IColetorRunner coletor;
        private readonly PromotionsDealer _promotionsDealer = new PromotionsDealer();

        public ProdutosController(IProdutoRepo produtoRepo, IPesquisaRepo pesquisaRepo, IColetorRunner coletorRunner)
        {
            _produtosRepo = produtoRepo;
            _pesquisaRepo = pesquisaRepo;
            coletor = coletorRunner;
        }

        [HttpGet("CollectAndStore/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosAndStore(string produtoNome)
        {
            coletor.CollectAndStore(produtoNome);

            var produtos = _produtosRepo.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("DB/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosFromDB(string produtoNome)
        {

            var produtos = _produtosRepo.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetProdutos(string produtoNome)
        {
            var produtos = coletor.CollectProducts(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("Promocao/{produtoNome}")]
        public ActionResult<IEnumerable<Promotion>> GetPromocoes(string produtoNome)
        {
            var produtos = _produtosRepo.GetProdutosByName(produtoNome);
            var pesquisa = _pesquisaRepo.GetPesquisaByName(produtoNome);

            return Ok(_promotionsDealer.GetPromotion(produtos, pesquisa));
        }
    }
}
