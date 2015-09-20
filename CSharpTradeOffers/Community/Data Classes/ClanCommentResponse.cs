using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ClanCommentResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("start")]
        public int Start { get; set; }
        [JsonProperty("pagesize")]
        public string PageSize { get; set; }
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }
        [JsonProperty("has_upvoted")]
        public int HasUpvoted { get; set; }
        [JsonProperty("comments_html")]
        public string CommentsHtml { get; set; }
        [JsonProperty("timelastpost")]
        public int TimeLastPost { get; set; }
    }
}
