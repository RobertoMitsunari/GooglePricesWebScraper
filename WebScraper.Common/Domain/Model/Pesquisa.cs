using Newtonsoft.Json;
using System;

namespace WebScraper.Common.Domain.Model
{
    public class Pesquisa
    {
        public Pesquisa()
        {

        }

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

        //public double MenorValor { get; set; }

        //public double Media { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
