using System;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Domain.Model;
using WebScraper.Common.Domain.Model;
using WebScraper.Common.Model;

namespace WebScraper.Api.Application
{
    public class PromotionsDealer
    {
        public List<Promotion> GetPromotion(IEnumerable<Produto> produtos, Pesquisa pesquisa)
        {
            var promocao = new List<Promotion>();

            produtos = produtos.ToList().Where(p => p.Preco < pesquisa.Media);

            foreach(Produto produto in produtos)
            {
                if (produto.Preco == pesquisa.MenorValor)
                {
                    promocao.Add(new Promotion(produto, "Menor valor"));
                }
                else
                {
                    promocao.Add(new Promotion(produto, "Abaixo da média"));
                }
            }

            return promocao;
        }
    }
}
