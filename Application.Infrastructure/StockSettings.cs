 using Newtonsoft.Json;

namespace Application.Infrastructure
{ 
    public class StockSettings
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonProperty(PropertyName = "userid")]
        public int UserId { get; set; }
        
        [JsonProperty(PropertyName = "stocktickersecond")]
        public int StockTickerSecond { get; set; }
    }
}