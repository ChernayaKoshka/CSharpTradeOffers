using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    public class ICSGOServers_730Handler
    {
        private const string BaseUrl = "https://api.steampowered.com/ICSGOServers_730/";
        private readonly string _apiKey;

        public ICSGOServers_730Handler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public dynamic GetGameServersStatus()
        {
            const string url = BaseUrl + "GetGameServersStatus/v1";
            var data = new Dictionary<string, string> {{"key", _apiKey}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data));
        }
    }
}
