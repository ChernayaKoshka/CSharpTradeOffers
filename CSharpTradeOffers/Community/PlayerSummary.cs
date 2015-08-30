using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Player")]
    public class PlayerSummary
    {
        public string steamid { get; set; }

        public int communityvisibilitystate { get; set; }

        public int profilestate { get; set; }

        public string personaname { get; set; }

        public int lastlogoff { get; set; }

        public string profileurl { get; set; }

        public string avatar { get; set; }

        public string avatarmedium { get; set; }

        public string avatarfull { get; set; }

        public int personastate { get; set; }

        public string primaryclanid { get; set; }

        public int timecreated { get; set; }

        public int personastateflags { get; set; }

        public string loccountrycode { get; set; }
    }
}