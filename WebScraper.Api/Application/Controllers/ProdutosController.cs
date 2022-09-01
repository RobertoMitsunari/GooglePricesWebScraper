using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WebScraper.Api.Application;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Domain.Model;
using WebScraper.Api.Infra;
using WebScraper.Common.Model;

namespace WebScraper.Api.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IColetorRunner _coletor;
        private readonly PromotionsDealer _promotionsDealer = new PromotionsDealer();
        private readonly DatabaseHelper _databaseHelper;
        private readonly IPesquisaFacade _pesquisaFacade;

        public ProdutosController(IColetorRunner coletor, IServiceScopeFactory scopeFactory, IPesquisaFacade pesquisaFacade)
        {
            _coletor = coletor;
            _databaseHelper = new DatabaseHelper(scopeFactory);
            _pesquisaFacade = pesquisaFacade;
        }

        [HttpGet("CollectAndStore/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosAndStore(string produtoNome)
        {
            _coletor.CollectAndStore(produtoNome);

            var produtos = _databaseHelper.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("DB/{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetAllProdutosFromDB(string produtoNome)
        {
            var produtos = _databaseHelper.GetProdutosByName(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("{produtoNome}")]
        public ActionResult<IEnumerable<Produto>> GetProdutos(string produtoNome)
        {
            var produtos = _coletor.CollectProducts(produtoNome);

            return Ok(produtos);
        }

        [HttpGet("promocao/{produtoNome}")]
        public ActionResult<IEnumerable<Promotion>> GetPromocoes(string produtoNome)
        {
            var pesquisa = _databaseHelper.GetPesquisaByName(produtoNome);

            if(pesquisa is null)
            {
                return NotFound();
            }

            var produtos = _databaseHelper.GetProdutosByName(produtoNome);

            return Ok(_promotionsDealer.GetPromotion(produtos, pesquisa));
        }

        [HttpGet("pesquisa/{produtoNome}")]
        public ActionResult<IEnumerable<Promotion>> SetPesquisa(string produtoNome)
        {
            var pesquisa = _databaseHelper.GetPesquisaByName(produtoNome);

            if (!(pesquisa is null))
            {
                return NotFound();
            }

            _coletor.UpdateOrInsertPesquisa(pesquisa, produtoNome);

            return Ok(pesquisa);
        }

        /*
         * TODOS:
         * PROBLEMA DE CONCORRENCIA (feito?)
        Cadastrar pesquisas sem ter q coletar
        Melhorar coletor para adicionar uma nova coletagem para pegar produtos fora da área de anúncios
        Melhorar níveis de promoção para calcular a porcentagem que o produto esta abaixo da media
        Melhorar filtro do coletor 
        Add endpoint que pega a melhor promo
        Add endpoint que pega a melhores promos (talvez n precise)
        */
    }
}
