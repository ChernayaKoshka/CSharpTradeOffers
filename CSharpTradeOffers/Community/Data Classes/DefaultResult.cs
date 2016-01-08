using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class DefaultResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
