using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeOffers
    {
        [JsonProperty("response")]
        public TradeOffersList Response { get; set; }
    }
}