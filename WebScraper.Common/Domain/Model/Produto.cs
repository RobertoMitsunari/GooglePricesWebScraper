using Newtonsoft.Json;
using System;

namespace WebScraper.Common.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Texto { get; set; }
        public string Preco { get; set; }
        public string Site { get; set; }
        public string Link { get; set; }
        public DateTime DataPesquisa { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
