using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Hadles Account Recovery tasks
    /// </summary>
    public class IAccountRecoverServiceHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/IAccountRecoveryService/";
        private readonly string _apiKey;

        /// <summary>
        /// Initializes the object with the api key
        /// </summary>
        /// <param name="apiKey"></param>
        public IAccountRecoverServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="loginUserList">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <param name="installConfig">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <param name="shaSentryFile">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <param name="machineId">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <returns>I forgot or it's obvious. TODO: Add better documentation</returns>
        public dynamic ReportAccountRecoveryData(string loginUserList, string installConfig, string shaSentryFile,
            string machineId)
        {
            const string url = BaseUrl + "ReportAccountRecoveryData/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"loginuser_list", loginUserList},
                {"install_config", installConfig},
                {"shasentryfile", shaSentryFile},
                {"machineid", machineId}
            };

            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "POST", data, null, false));
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="requesthandle">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <param name="authContainer">I forgot or it's obvious. TODO: Add better documentation</param>
        /// <returns></returns>
        public dynamic RetrieveAccountRecoveryData(string requesthandle, CookieContainer authContainer)
        {
            const string url = BaseUrl + "RetrieveAccountRecoveryData/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"requesthandle", requesthandle}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "POST", data, null, false));
        }
    }
}
