using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class GetTradeOffersSummaryBaseResponse
    {
        [JsonProperty("response")]
        public GetTradeOffersSummaryResponse Response { get; set; }
    }
}