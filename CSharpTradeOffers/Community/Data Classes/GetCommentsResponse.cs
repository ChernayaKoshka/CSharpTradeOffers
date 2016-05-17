using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject("RootObject")]
    public class GetCommentsResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("pagesize")]
        public string Pagesize { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }

        [JsonProperty("has_upvoted")]
        public int HasUpvoted { get; set; }

        [JsonProperty("comments_html")]
        public string CommentsHtml { get; set; }

        [JsonProperty("timelastpost")]
        public int Timelastpost { get; set; }
    }
}
