using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ForumInfo
    {
        [JsonProperty("topic_permissions")]
        public TopicPermissions TopicPermissions { get; set; }

        [JsonProperty("original_post")]
        public int OriginalPoster { get; set; }

        [JsonProperty("forum_appid")]
        public int ForumAppid { get; set; }

        [JsonProperty("forum_public")]
        public int ForumPublic { get; set; }

        [JsonProperty("forum_type")]
        public string ForumType { get; set; }

        [JsonProperty("forum_gidfeature")]
        public string ForumGidfeature { get; set; }
    }

    public class TopicPermissions
    {
        [JsonProperty("can_view")]
        public int CanView { get; set; }

        [JsonProperty("can_post")]
        public int CanPost { get; set; }

        [JsonProperty("can_reply")]
        public int CanReply { get; set; }

        [JsonProperty("can_moderate")]
        public int CanModerate { get; set; }

        [JsonProperty("can_edit_others_posts")]
        public int CanEditOthersPosts { get; set; }

        [JsonProperty("can_purge_topics")]
        public int CanPurgeTopics { get; set; }

        [JsonProperty("is_banned")]
        public int IsBanned { get; set; }

        [JsonProperty("can_delete")]
        public int CanDelete { get; set; }

        [JsonProperty("can_edit")]
        public int CanEdit { get; set; }
    }
}
