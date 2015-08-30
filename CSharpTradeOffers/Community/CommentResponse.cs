using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class CommentResponse
    {

        public bool success { get; set; }

        public string name { get; set; }

        public int start { get; set; }

        public string pagesize { get; set; }

        public int total_count { get; set; }

        public int upvotes { get; set; }

        public int has_upvoted { get; set; }

        public string comments_html { get; set; }

        public int timelastpost { get; set; }
    }
}