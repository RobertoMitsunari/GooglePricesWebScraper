using Newtonsoft.Json;
using WebScraper.Common.Model;

namespace WebScraper.Api.Domain.Model
{
    public class Promotion
    {
        public Produto Produto { get; set; }
        public string Tipo { get; set; }

        public Promotion(Produto produto, string tipo)
        {
            Produto = produto;
            Tipo = tipo;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
