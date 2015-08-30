using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class MarketValue
    {

        public bool success { get; set; }

        public string lowest_price { get; set; }

        public string volume { get; set; }

        public string median_price { get; set; }
    }
}