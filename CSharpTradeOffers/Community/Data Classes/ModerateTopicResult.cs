using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ModerateTopicResult
    {
        [JsonProperty("resolved_count")]
        public int ResolvedCount { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}

