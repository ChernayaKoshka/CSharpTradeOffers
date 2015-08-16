using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class ICSGOServers_730Handler
    {
        private const string BaseUrl = "https://api.steampowered.com/ICSGOServers_730/";
        private readonly string _apiKey;

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="apiKey"></param>
        public ICSGOServers_730Handler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <returns></returns>
        public dynamic GetGameServersStatus()
        {
            const string url = BaseUrl + "GetGameServersStatus/v1";
            var data = new Dictionary<string, string> {{"key", _apiKey}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data));
        }
    }
}
