using System;
using System.Collections.Generic;
using WebScraper.Common.Model;

namespace WebScraper.Coletor.Application
{
    public class Cleaner
    {
        public List<Produto> ClearData(List<Produto> produtos, string search)
        {
            var cleanedProducts = new List<Produto>();

            foreach (Produto produto in produtos)
            {
                if (ContainsAllSearchText(produto, search))
                {
                    produto.Nome = search.ToLower();
                    cleanedProducts.Add(produto);
                }
            }

            return cleanedProducts;
        }

        private bool ContainsAllSearchText(Produto produto, string search)
        {
            string[] words = search.Split(' ');

            search.Replace("-", " ");

            foreach (string word in words)
            {
                if(!produto.Texto.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
