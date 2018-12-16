using Newtonsoft.Json;

namespace Application.Infrastructure
{
    public class Stocks
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "stockcode")]
        public string StockCode { get; set; }

        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }

        [JsonProperty(PropertyName = "isactive")]
        public bool IsActive { get; set; }
        
        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }
    }
}