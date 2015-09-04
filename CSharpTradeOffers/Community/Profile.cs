namespace CSharpTradeOffers.Community
{
    public class Profile
    {
        public const string type = "profileSave";

        public string weblink_1_title { get; set; }
        public string weblink_1_url { get; set; }

        public string weblink_2_title { get; set; }
        public string weblink_2_url { get; set; }

        public string weblink_3_title { get; set; }
        public string weblink_3_url { get; set; }

        public string personaName { get; set; }
        public string real_name { get; set; }

        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }

        public string customURL { get; set; }

        public string summary { get; set; }

        public ulong? favorite_badge_badgeid { get; set; }
        public ulong? favorite_badge_communityitemid { get; set; }

        public ulong? primary_group_steamid { get; set; }
    }
}
