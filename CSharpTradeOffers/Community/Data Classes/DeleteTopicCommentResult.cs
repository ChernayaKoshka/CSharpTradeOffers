using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class DeleteTopicCommentResult
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
        //public Comments_Raw comments_raw { get; set; }
    }

    //figure this part out later. Implement as a dictionary maybe?
    /*public class Comments_Raw
    {
        public _458605613398425962 _458605613398425962 { get; set; }
    }

    public class _458605613398425962
    {
        public string text { get; set; }
        public string author { get; set; }
    }*/
}
