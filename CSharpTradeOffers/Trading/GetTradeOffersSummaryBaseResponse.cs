using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class GetTradeOffersSummaryBaseResponse
    {
        public GetTradeOffersSummaryResponse response { get; set; }
    }
}