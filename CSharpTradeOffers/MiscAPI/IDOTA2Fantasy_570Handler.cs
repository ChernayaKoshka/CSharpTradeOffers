using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    public class IDOTA2Fantasy_570Handler
    {
        private const string BaseUrl = "https://api.steampowered.com/IDOTA2Fantasy_570/";
        private readonly string _apiKey;

        public IDOTA2Fantasy_570Handler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public GetFantasyPlayerStatsResult GetFantasyPlayerStats(uint fantasyLeagueId, uint startTime, uint endTime,
            ulong matchId, uint seriesId, uint playerAccountId)
        {
            const string url = BaseUrl + "GetFantasyPlayerStats/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"FantasyLeagueID", fantasyLeagueId.ToString()},
                {"StartTime", startTime.ToString()},
                {"EndTime", endTime.ToString()},
                {"matchid", matchId.ToString()},
                {"SeriesID", seriesId.ToString()},
                {"PlayerAccountID", playerAccountId.ToString()}
            };

            return
                JsonConvert.DeserializeObject<GetFantasyPlayerStatsBaseResult>(Web.Fetch(url, "GET", data, null, false))
                    .result;
        }

        public GetPlayerOfficialInfoResult GetPlayerOfficialInfo(uint accountid)
        {
            const string url = BaseUrl + "GetPlayerOfficialInfo/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"accountid", accountid.ToString()}};
            return
                JsonConvert.DeserializeObject<GetPlayerOfficalInfoBaseResult>(Web.Fetch(url, "GET", data, null, false))
                    .result;
        }

    }

    #region GetPlayerOfficialInfoResult
    [JsonObject(Title = "RootObject")]
    public class GetPlayerOfficalInfoBaseResult
    {
        public GetPlayerOfficialInfoResult result { get; set; }
    }

    [JsonObject(Title = "Result")]
    public class GetPlayerOfficialInfoResult
    {
        public string Name { get; set; }
        public string TeamName { get; set; }
        public string TeamTag { get; set; }
        public string Sponsor { get; set; }
        public int FantasyRole { get; set; }
    }
    #endregion

    #region GetFantasyPlayerStatsResult
    public class StatsList
    {
        public int PlayerAccountID { get; set; }
        public string PlayerName { get; set; }
        public int Role { get; set; }
        public int Score { get; set; }
        public int Matches { get; set; }
        public double AverageLevel { get; set; }
        public double AverageKills { get; set; }
        public double AverageDeaths { get; set; }
        public double AverageAssists { get; set; }
        public double AverageLastHits { get; set; }
        public double AverageDenies { get; set; }
        public double AverageGPM { get; set; }
        public double AverageXPPM { get; set; }
        public double AverageStuns { get; set; }
        public double AverageHealing { get; set; }
        public double AverageTowerKills { get; set; }
        public double AverageRoshanKills { get; set; }
    }

    [JsonObject(Title = "Result")]
    public class GetFantasyPlayerStatsResult
    {
        public List<StatsList> StatsList { get; set; }
        public bool success { get; set; }
    }

    [JsonObject(Title = "RootObject")]
    public class GetFantasyPlayerStatsBaseResult
    {
        public GetFantasyPlayerStatsResult result { get; set; }
    }
    #endregion
}
