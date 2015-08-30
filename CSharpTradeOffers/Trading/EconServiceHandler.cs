using System.Collections.Generic;
using System.Linq;
using System.Net;
using CSharpTradeOffers.Configuration;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Handles EconService related tasks such as trade offers.
    /// </summary>
    public class EconServiceHandler
    {
        private readonly string _apiKey ;

        private const string BaseUrl = "https://api.steampowered.com/IEconService/";

        public EconServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Requests all trade offers.
        /// </summary>
        /// <param name="data">A dictionary containing the URL params found here: https://developer.valvesoftware.com/wiki/Steam_Web_API/IEconService at GetTradeOffers (v1).
        /// ex: [leftarrow]"get_sent_offers","1"[rightarrow]. Please note, [leftarrow] and [rightarrow] correspond to the keys on the keyboard.</param>
        /// <returns></returns>
        public TradeOffersList GetTradeOffers(Dictionary<string, string> data)
        {
            const string url = BaseUrl + "GetTradeOffers/v1/";
            data.Add("key", _apiKey);
            data.Add("format", "json");
            return
                JsonConvert.DeserializeObject<TradeOffers>(
                    WebUtility.UrlDecode(Web.Fetch(url, "GET", data))).response;
        }

        /// <summary>
        /// Requests a single trade offer based on the tradeofferid.
        /// </summary>
        /// <param name="tradeofferid">The tradeofferid to request information on.</param>
        /// <param name="language">The language to use. Default: english</param>
        /// <returns>A CEConTradeOffer object.</returns>
        public CEconTradeOffer GetTradeOffer(string tradeofferid, string language = "english")
        {
            const string url = BaseUrl + "GetTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"language", language},
                {"format", "json"}
            };
            return
                JsonConvert.DeserializeObject<CEconTradeOffer>(
                    WebUtility.UrlDecode(Web.Fetch(url, "GET", data)));
        }

        /// <summary>
        /// Declines a trade offer that was sent to you.
        /// </summary>
        /// <param name="tradeofferid">The ID of the offer to decline.</param>
        /// <returns></returns>
        public string DeclineTradeOffer(string tradeofferid)
        {
            const string url = BaseUrl + "DeclineTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"format", "json"}
            };
            return Web.Fetch(url, "POST", data);
        }

        /// <summary>
        /// Cancels a trade offer based on the specified tradeofferid.
        /// </summary>
        /// <param name="tradeofferid">The ID of the offer to cancel.</param>
        /// <returns></returns>
        public string CancelTradeOffer(string tradeofferid)
        {
            const string url = BaseUrl + "CancelTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"format", "json"}
            };
            return WebUtility.UrlDecode(Web.Fetch(url, "POST", data));
        }

        /// <summary>
        /// Accepts a specified trade offer.
        /// </summary>
        /// <param name="tradeId">A TradeId object containing the id of the trade to accept.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="partnerId">The AccountId of the person to trade with.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <returns>The TradeId of the offer that was accepted.</returns>
        public TradeId AcceptTradeOffer(TradeId tradeId, CookieContainer container, uint partnerId, string serverid)
        {
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            const string url = "https://steamcommunity.com/tradeoffer/{0}/accept";
            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"tradeofferid", tradeId.tradeid.ToString()},
                {"partner", SteamIdOperations.ConvertAccountIdtoUInt64(partnerId).ToString()},
                {"captcha", ""}
            };
            return
                JsonConvert.DeserializeObject<TradeId>(
                    WebUtility.UrlDecode(Web.Fetch(string.Format(url, tradeId.tradeid), "POST",
                        data, container, false, "https://steamcommunity.com/tradeoffer/" + tradeId.tradeid + "/")));
        }

        /// <summary>
        /// Accepts a specified trade offer.
        /// </summary>
        /// <param name="tradeId">A ulong representing the trade to accept.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="partnerId">The AccountId of the person to trade with.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <returns>The TradeId of the offer that was accepted.</returns>
        public TradeId AcceptTradeOffer(ulong tradeId, CookieContainer container, uint partnerId, string serverid)
        {
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            const string url = "https://steamcommunity.com/tradeoffer/{0}/accept";
            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"tradeofferid", tradeId.ToString()},
                {"partner", SteamIdOperations.ConvertAccountIdtoUInt64(partnerId).ToString()},
                {"captcha", ""}
            };
            return
                JsonConvert.DeserializeObject<TradeId>(
                    WebUtility.UrlDecode(Web.Fetch(string.Format(url, tradeId), "POST",
                        data, container, false, "https://steamcommunity.com/tradeoffer/" + tradeId + "/")));
        }

        /// <summary>
        /// Sends a trade offer to the specified recipient. 
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person to send the offer to.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="tradeoffermessage">An optional message to be sent with the offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters.</param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse SendTradeOffer(ulong partnerSid, CookieContainer container, string tradeoffermessage,
            string serverid, TradeOffer offer)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });

            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", ""},
                {"trade_offer_create_params", "{}"}
            };
            return JsonConvert.DeserializeObject<SendOfferResponse>(Web.Fetch(url, "POST", data, container, false,
                "https://steamcommunity.com/tradeoffer/new/?partner=" +
                SteamIdOperations.ConvertSteamIdToAccountId(SteamIdOperations.ConvertUlongToSteamId(partnerSid))));
        }

        /// <summary>
        /// Modifies an existing trade offer.
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person whose offer will be modified.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="tradeoffermessage">An otpional message to be sent with the trade offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="tradeofferidCountered">The TradeId of the offer to counter or modify.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters. </param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse ModifyTradeOffer(ulong partnerSid, CookieContainer container, string tradeoffermessage,
            string serverid, uint tradeofferidCountered, TradeOffer offer)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });

            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", ""},
                {"trade_offer_create_params", "{}"},
                {"tradeofferid_countered", tradeofferidCountered.ToString()}
            };
            return JsonConvert.DeserializeObject<SendOfferResponse>(Web.Fetch(url, "POST", data, container, false,
                "https://steamcommunity.com/tradeoffer/" + tradeofferidCountered + "/"));
        }

        /// <summary>
        /// Requestss TradeOffer statistics. ie: historical_received_count
        /// </summary>
        /// <param name="timeLastVisit">Unix time for historical cutoff.</param>
        /// <returns></returns>
        public GetTradeOffersSummaryResponse GetTradeOffersSummary(uint timeLastVisit)
        {
            const string url = BaseUrl + "GetTradeOffersSummary/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"time_last_visit", timeLastVisit.ToString()}
            };
            return
                JsonConvert.DeserializeObject<GetTradeOffersSummaryBaseResponse>(Web.Fetch(url, "GET", data)).response;
        }
    }
}