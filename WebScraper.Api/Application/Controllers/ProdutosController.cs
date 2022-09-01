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
        private readonly IPesquisaRepo _pesquisaRepo; //Usar o DatabaseHelper aqui tmb (como um singleton?)
        private readonly IColetorRunner _coletor;
        private readonly PromotionsDealer _promotionsDealer = new PromotionsDealer();

        public ProdutosController(IProdutoRepo produtoRepo, IPesquisaRepo pesquisaRepo, IColetorRunner coletor)
        {
            _produtosRepo = produtoRepo;
            _pesquisaRepo = pesquisaRepo;
            //coletor = new ColetorRunner(produtoRepo, pesquisaRepo);
            _coletor = coletor;
        }

        [HttpGet("CollectAndStore/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosAndStore(string produtoNome)
        {
            _coletor.CollectAndStore(produtoNome);

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
            var produtos = _coletor.CollectProducts(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("Promocao/{produtoNome}")]
        public ActionResult<IEnumerable<Promotion>> GetPromocoes(string produtoNome)
        {
            var produtos = _produtosRepo.GetProdutosByName(produtoNome);
            var pesquisa = _pesquisaRepo.GetPesquisaByName(produtoNome);

            return Ok(_promotionsDealer.GetPromotion(produtos, pesquisa));
        }

        /*
         * TODOS:
         * PROBLEMA DE CONCORRENCIA
        Cadastrar pesquisas sem ter q coletar
        Melhorar níveis de promoção para calcular a porcentagem que o produto esta abaixo da media
        Melhorar filtro do coletor
        Add endpoint que pega a melhor promo
        Add endpoint que pega a melhores promos (talvez n precise)
        */
    }
}
