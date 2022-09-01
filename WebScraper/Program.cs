using System;
using System.Collections.Generic;
using WebScraper.Application;
using WebScraper.Common.Model;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Digite o produto");
            var pesquisa = Console.ReadLine().Replace(" ","+");

            var url = $"https://www.google.com.br/search?q={pesquisa}&hl=pt-BR&tbm=shop";

            var scrapEngine = new ScrapEngine();

            List<Produto> produtops = scrapEngine.Coletar(url);

            foreach(Produto produto in produtops)
            {
                Console.WriteLine(produto.ToString());
            }   
        }
    }
}
