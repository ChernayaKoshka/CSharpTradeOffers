using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetTopicsHtmlResult
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("topics_html")]
        public string TopicsHtml { get; set; }
    }
}
