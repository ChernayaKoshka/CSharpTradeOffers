using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeId
    {

        public ulong tradeid { get; set; }
    }
}