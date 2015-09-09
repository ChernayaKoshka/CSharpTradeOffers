using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class Trade
    {
        [JsonProperty("tradeid")]
        public ulong TradeId { get; set; }
    }
}