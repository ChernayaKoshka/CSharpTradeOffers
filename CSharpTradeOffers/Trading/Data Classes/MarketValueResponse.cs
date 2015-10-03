using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class MarketValueResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("lowest_price")]
        public string LowestPrice { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("median_price")]
        public string MedianPrice { get; set; }
    }
}