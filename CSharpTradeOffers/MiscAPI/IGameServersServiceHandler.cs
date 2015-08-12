using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CSharpTradeOffers.MiscAPI
{
    // ReSharper disable once InconsistentNaming
    public class IGameServersServiceHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/IGameServersService/";
        private readonly string _apiKey;

        public IGameServersServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Gets a list of servers owned by the account
        /// </summary>
        /// <returns>Unknown, was unable to succeed since my account owns no servers. Returns dynamic.</returns>
        public dynamic GetAccountList()
        {
            const string url = BaseUrl + "GetAccountList/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data, null, false));
        }

        /// <summary>
        /// Creates a persistent game server account
        /// </summary>
        /// <param name="appId">App to use account for</param>
        /// <param name="memo">Memo to set on new account</param>
        /// <returns>A CreateAccountResponse object.</returns>
        public CreateAccountResponse CreateAccount(uint appId, string memo)
        {
            const string url = BaseUrl + "CreateAccount/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"appid", appId.ToString()}, {"memo", memo}};
            return
                JsonConvert.DeserializeObject<CreateAccountBaseResponse>(Web.Fetch(url, "POST", data, null, false))
                    .response;
        }

        /// <summary>
        /// This method changes the memo associated with the game server account. 
        /// Memos do not affect the account in any way. 
        /// The memo shows up in the GetAccountList response and serves only as a reminder of what the account is used for.
        /// </summary>
        /// <param name="steamId">The SteamId64 of the game server.</param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public dynamic SetMemo(ulong steamId, string memo)
        {
            const string url = BaseUrl + "SetMemo/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"steamid", steamId.ToString()},
                {"memo", memo}
            };
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "POST", data, null, false));
        }

        /// <summary>
        /// Resets the login token for the specified game server
        /// </summary>
        /// <param name="steamId">Server SteamId64 to reset the token of.</param>
        /// <returns>A ResetLoginTokenResponse containing the new token.</returns>
        public ResetLoginTokenResponse ResetLoginToken(ulong steamId)
        {
            const string url = "ResetLoginToken/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"steamid", steamId.ToString()}};
            return
                JsonConvert.DeserializeObject<ResetLoginTokenBaseResponse>(Web.Fetch(url, "POST", data, null, false))
                    .response;
        }

        /// <summary>
        /// Gets the public information about the specified game server account.
        /// </summary>
        /// <param name="steamId">Game server SteamId64 to get info from.</param>
        /// <returns></returns>
        public GetAccountPublicInfoResponse GetAccountPublicInfo(ulong steamId)
        {
            const string url = BaseUrl + "GetAccountPublicInfo/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"steamid", steamId.ToString()}};
            return
                JsonConvert.DeserializeObject<GetAccountPublicInfoBaseResponse>(Web.Fetch(url, "GET", data, null, false))
                    .response;
        }

        /// <summary>
        /// Gets a list of server SteamIDs from a list of server IPs
        /// </summary>
        /// <param name="serverIps">A list of IPs to get the SteamIDs for.</param>
        /// <returns>Unknown</returns>
        public dynamic GetServerSteamIdsByIp(List<string> serverIps)
        {
            const string url = BaseUrl + "GetServerSteamIDsByIP/v1/";
            var data = new Dictionary<string, string> {{"key", _apiKey}, {"server_ips", CommaDelimitIp(serverIps)}};
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data, null, false));
        }

        /// <summary>
        /// Gets a list of server SteamIDs from a list of server IPs
        /// </summary>
        /// <param name="serverSteamIds">A list of server Sid64s to get the IPs of.</param>
        /// <returns>Unknown</returns>
        public dynamic GetServerIPsBySteamId(List<ulong> serverSteamIds)
        {
            const string url = BaseUrl + "GetServerIPsBySteamID/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"server_ips", CommaDelimitId(serverSteamIds)}
            };
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data, null, false));
        }

        private static string CommaDelimitIp(IEnumerable<string> toDelimit)
        {
            string returned = toDelimit.Aggregate("", (current, @String) => current + (@String + ","));
            return returned.Substring(0, returned.Length - 1);
        }

        private static string CommaDelimitId(IEnumerable<ulong> toDelimit)
        {
            string returned = toDelimit.Aggregate("", (current, @Ulong) => current + (@Ulong + ","));
            return returned.Substring(0, returned.Length - 1);
        }
    }

    #region GetAccountPublicInfoResponse
    [JsonObject(Title = "RootObject")]
    public class GetAccountPublicInfoBaseResponse
    {
        public GetAccountPublicInfoResponse response { get; set; }
    }

    [JsonObject(Title = "Response")]
    public class GetAccountPublicInfoResponse
    {
        public string steamid { get; set; }
        public int appid { get; set; }
    }
    #endregion

    #region ResetLoginTokenResponse
    [JsonObject(Title = "RootObject")]
    public class ResetLoginTokenBaseResponse
    {
        public ResetLoginTokenResponse response { get; set; }
    }

    [JsonObject(Title = "Response")]
    public class ResetLoginTokenResponse
    {
        public string login_token { get; set; }
    }
    #endregion

    #region CreateAccountResponse
    [JsonObject(Title = "RootObject")]
    public class CreateAccountBaseResponse
    {
        public CreateAccountResponse response { get; set; }
    }

    [JsonObject(Title = "Response")]
    public class CreateAccountResponse
    {
        public string steamid { get; set; }
        public string login_token { get; set; }
    }

    #endregion
}
