using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetPlayerSummariesV2Result
    {
        public ResponseV2 Response { get; set; }
    }
}