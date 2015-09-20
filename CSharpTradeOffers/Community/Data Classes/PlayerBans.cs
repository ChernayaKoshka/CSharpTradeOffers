using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Player")]
    public class PlayerBans
    {
        public string SteamId { get; set; }

        public bool CommunityBanned { get; set; }

        // ReSharper disable once InconsistentNaming
        public bool VACBanned { get; set; }

        // ReSharper disable once InconsistentNaming
        public int NumberOfVACBans { get; set; }

        public int DaysSinceLastBan { get; set; }

        public int NumberOfGameBans { get; set; }

        public string EconomyBan { get; set; }
    }
}