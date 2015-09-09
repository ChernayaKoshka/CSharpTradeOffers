using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Player")]
    public class PlayerBans
    {
        public string SteamId { get; set; }

        public bool CommunityBanned { get; set; }

        public bool VACBanned { get; set; }

        public int NumberOfVACBans { get; set; }

        public int DaysSinceLastBan { get; set; }

        public int NumberOfGameBans { get; set; }

        public string EconomyBan { get; set; }
    }
}