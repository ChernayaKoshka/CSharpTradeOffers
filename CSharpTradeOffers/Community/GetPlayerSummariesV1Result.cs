using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetPlayerSummariesV1Result
    {

        public ResponseV1 Response { get; set; }
    }
}