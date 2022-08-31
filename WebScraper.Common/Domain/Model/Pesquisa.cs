using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebScraper.Common.Model;

namespace WebScraper.Common.Domain.Model
{
    public class Pesquisa
    {
        public Pesquisa() { }

        public Pesquisa(string name)
        {
            Name = name;
            DataPesquisa = DateTime.Now;
            DataCadastro = DateTime.Now;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DataPesquisa { get; set; }

        public DateTime DataCadastro { get; set; }

        public decimal MenorValor { get; set; }

        public decimal Media { get; set; }

        public void CalculaValores(IEnumerable<Produto> produtos)
        {
            GetPrices(this, produtos.GetEnumerator(), 0, MenorValor, 0);
        }

        private Pesquisa GetPrices(Pesquisa pesquisa, IEnumerator<Produto> produtos, decimal media, decimal min, int interator)
        {
            if (produtos.MoveNext())
            {
                var price = produtos.Current.Preco;

                if (min > price || min == 0)
                {
                    min = price;
                }

                media += produtos.Current.Preco;

                return GetPrices(pesquisa, produtos, media, min, interator + 1);
            }
            else
            {
                if (pesquisa.Media == 0)
                {
                    pesquisa.Media = media / interator;
                }
                else
                {
                    pesquisa.Media = (pesquisa.Media + (media / interator)) / 2;
                }

                pesquisa.MenorValor = min;

                return pesquisa;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
