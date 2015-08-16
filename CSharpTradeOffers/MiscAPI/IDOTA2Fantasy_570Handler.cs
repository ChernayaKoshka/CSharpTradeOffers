using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class IDOTA2Fantasy_570Handler
    {
        private const string BaseUrl = "https://api.steampowered.com/IDOTA2Fantasy_570/";
        private readonly string _apiKey;

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="apiKey"></param>
        public IDOTA2Fantasy_570Handler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="fantasyLeagueId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="matchId"></param>
        /// <param name="seriesId"></param>
        /// <param name="playerAccountId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
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
    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class GetPlayerOfficalInfoBaseResult
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public GetPlayerOfficialInfoResult result { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    [JsonObject(Title = "Result")]
    public class GetPlayerOfficialInfoResult
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string TeamTag { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string Sponsor { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int FantasyRole { get; set; }
    }
    #endregion

    #region GetFantasyPlayerStatsResult
    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class StatsList
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int PlayerAccountID { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string PlayerName { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int Role { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int Matches { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageLevel { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageKills { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageDeaths { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageAssists { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageLastHits { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageDenies { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageGPM { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageXPPM { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageStuns { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageHealing { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageTowerKills { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public double AverageRoshanKills { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    [JsonObject(Title = "Result")]
    public class GetFantasyPlayerStatsResult
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<StatsList> StatsList { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public bool success { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class GetFantasyPlayerStatsBaseResult
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public GetFantasyPlayerStatsResult result { get; set; }
    }
    #endregion
}
