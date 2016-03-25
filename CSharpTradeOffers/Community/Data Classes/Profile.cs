using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class Profile
    {
        public const string Type = "profileSave";

        // ToDo: Better Property Names for the weblinks need to be checked with the input ids from the profile edit page
        [JsonProperty("weblink_1_title")]
        public string Weblink1Title { get; set; }
        [JsonProperty("weblink_1_url")]
        public string Weblink1Url { get; set; }

        [JsonProperty("weblink_2_title")]
        public string Weblink2Title { get; set; }
        [JsonProperty("weblink_2_url")]
        public string Weblink2Url { get; set; }

        [JsonProperty("weblink_3_title")]
        public string Weblink3Title { get; set; }
        [JsonProperty("weblink_3_url")]
        public string Weblink3Url { get; set; }

        [JsonProperty("personaName")]
        public string PersonaName { get; set; }
        [JsonProperty("real_name")]
        public string RealName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("customURL")]
        public string CustomUrl { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("favorite_badge_badgeid")]
        public ulong? FavoriteBadgeBadgeId { get; set; }
        [JsonProperty("favorite_badge_communityitemid")]
        public ulong? FavoriteBadgeCommunityItemId { get; set; }

        [JsonProperty("primary_group_steamid")]
        public ulong? PrimaryGroupSteamId { get; set; }
    }
}
