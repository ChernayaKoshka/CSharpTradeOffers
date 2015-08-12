using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    class ICheatReportingServiceHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/ICheatReportingService/";

        private readonly string _apiKey;

        public ICheatReportingServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public dynamic ReportCheatData(ulong steamId, uint appid, string pathAndFilename, string webCheatUrl,
            ulong timeNow, ulong timeStarted, ulong timeStopped, string cheatname, uint gameProcessId,
            uint cheatProcessId)
        {
            const string url = BaseUrl + "ReportCheatData/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"steamid", steamId.ToString()},
                {"appid", appid.ToString()},
                {"pathandfilename", pathAndFilename},
                {"webcheaturl", webCheatUrl},
                {"time_now", timeNow.ToString()},
                {"time_started", timeStarted.ToString()},
                {"time_stopped", timeStopped.ToString()},
                {"cheatname", cheatname},
                {"game_process_id", gameProcessId.ToString()},
                {"cheat_process_id", cheatProcessId.ToString()}
            };

            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "POST", data, null, false));
        }
    }
}
