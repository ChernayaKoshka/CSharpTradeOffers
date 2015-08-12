using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    public class IAccountRecoverServiceHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/IAccountRecoveryService/";
        private readonly string _apiKey;

        public IAccountRecoverServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

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

        public dynamic RetrieveAccountRecoveryData(string requesthandle, CookieContainer authContainer)
        {
            const string url = BaseUrl + "RetrieveAccountRecoveryData/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"requesthandle", requesthandle}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "POST", data, null, false));
        }
    }
}
