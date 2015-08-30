using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeOffers
    {

        public TradeOffersList response { get; set; }
    }
}