using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    class IDOTA2MatchStats_570Handler
    {
        private readonly string _apiKey ;

        private const string BaseUrl = "https://api.steampowered.com/IDOTA2MatchStats_570/";

        public IDOTA2MatchStats_570Handler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public dynamic GetRealtimeStats(ulong serverSteamId)
        {
            const string url = BaseUrl + "GetRealtimeStats/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"server_steam_id", serverSteamId.ToString()}
            };
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data, null, false));
        }
    }
}
