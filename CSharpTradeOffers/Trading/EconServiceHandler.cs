using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CSharpTradeOffers.Web;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Handles EconService related tasks such as trade offers.
    /// </summary>
    public class EconServiceHandler
    {
        private readonly string _apiKey;
        private readonly Web.Web _web = new Web.Web(new SteamWebRequestHandler());

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
                    WebUtility.UrlDecode(_web.Fetch(url, "GET", data).ReadStream())).Response;
        }

        /// <summary>
        /// Requests a single trade offer based on the tradeofferid.
        /// </summary>
        /// <param name="tradeofferid">The tradeofferid to request information on.</param>
        /// <param name="language">The language to use. Default: english</param>
        /// <returns>A CEConTradeOffer object.</returns>
        public CEconTradeOffer GetTradeOffer(ulong tradeofferid, string language = "english")
        {
            const string url = BaseUrl + "GetTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid.ToString()},
                {"language", language},
                {"format", "json"}
            };
            return
                JsonConvert.DeserializeObject<CEconTradeOffer>(
                    WebUtility.UrlDecode(_web.Fetch(url, "GET", data).ReadStream()));
        }

        /// <summary>
        /// Declines a trade offer that was sent to you.
        /// </summary>
        /// <param name="tradeofferid">The ID of the offer to decline.</param>
        /// <returns></returns>
        public string DeclineTradeOffer(ulong tradeofferid)
        {
            const string url = BaseUrl + "DeclineTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid.ToString()},
                {"format", "json"}
            };
            return _web.Fetch(url, "POST", data).ReadStream();
        }

        /// <summary>
        /// Cancels a trade offer based on the specified tradeofferid.
        /// </summary>
        /// <param name="tradeofferid">The ID of the offer to cancel.</param>
        /// <returns></returns>
        public string CancelTradeOffer(ulong tradeofferid)
        {
            const string url = BaseUrl + "CancelTradeOffer/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid.ToString()},
                {"format", "json"}
            };
            return WebUtility.UrlDecode(_web.Fetch(url, "POST", data).ReadStream());
        }

        /// <summary>
        /// Accepts a specified trade offer.
        /// </summary>
        /// <param name="tradeId">A ulong representing the trade to accept.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="partnerId">The AccountId of the person to trade with.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <returns>The TradeId of the offer that was accepted.</returns>
        public Trade AcceptTradeOffer(uint tradeId, uint partnerId, CookieContainer container, string serverid)
        {
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            string sessionid = (from Cookie cookie in container.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            const string url = "https://steamcommunity.com/tradeoffer/{0}/accept";
            var data = new Dictionary<string, string>
            {
                {"sessionid", sessionid},
                {"serverid", serverid},
                {"tradeofferid", tradeId.ToString()},
                {"partner", IdConversions.AccountIdToUlong(partnerId).ToString()},
                {"captcha", string.Empty}
            };
            return
                JsonConvert.DeserializeObject<Trade>(
                    WebUtility.UrlDecode(_web.Fetch(string.Format(url, tradeId), "POST",
                        data, container, false, "https://steamcommunity.com/tradeoffer/" + tradeId + "/").ReadStream()));
        }

        /// <summary>
        /// Sends a trade offer to the specified recipient. 
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person to send the offer to.</param>
        /// <param name="tradeoffermessage">An optional message to be sent with the offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse SendTradeOffer(ulong partnerSid, string tradeoffermessage,
            string serverid, TradeOffer offer, CookieContainer container)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            string sessionid = (from Cookie cookie in container.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionid", sessionid},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", string.Empty},
                {"trade_offer_create_params", "{}"}
            };
            return
                JsonConvert.DeserializeObject<SendOfferResponse>(
                    _web.RetryFetch(TimeSpan.FromSeconds(10), 20, url, "POST", data, container, false,
                        "https://steamcommunity.com/tradeoffer/new/?partner=" +
                        IdConversions.UlongToAccountId(partnerSid))
                        .ReadStream());
        }

        /// <summary>
        /// Sends a trade offer to the specified recipient that's not on your friends list using the trade url. If is not the case, use SendTradeOffer function.
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person to send the offer to.</param>
        /// <param name="token">The token part from the recipient's trade url. Example: a1b2cdEF</param>
        /// <param name="tradeoffermessage">An optional message to be sent with the offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse SendTradeOfferWithLink(ulong partnerSid, string token, string tradeoffermessage,
            string serverid, TradeOffer offer, CookieContainer container)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            string sessionid = (from Cookie cookie in container.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            CEconTradeOffer offerToken = new CEconTradeOffer { TradeOfferAccessToken = token };

            var data = new Dictionary<string, string>
            {
                {"sessionid", sessionid},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", string.Empty},
                {"trade_offer_create_params", JsonConvert.SerializeObject(offerToken)}
            };
            return
                JsonConvert.DeserializeObject<SendOfferResponse>(
                    _web.RetryFetch(TimeSpan.FromSeconds(10), 20, url, "POST", data, container, false,
                        $"https://steamcommunity.com/tradeoffer/new/?partner={IdConversions.UlongToAccountId(partnerSid)}&token={token}").ReadStream());
        }

        /// <summary>
        /// Modifies an existing trade offer.
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person whose offer will be modified.</param>
        /// <param name="tradeoffermessage">An otpional message to be sent with the trade offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="tradeofferidCountered">The TradeId of the offer to counter or modify.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters. </param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse ModifyTradeOffer(ulong partnerSid, string tradeoffermessage,
            string serverid, uint tradeofferidCountered, TradeOffer offer, CookieContainer container)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            string sessionid = (from Cookie cookie in container.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionid", sessionid},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", string.Empty},
                {"trade_offer_create_params", "{}"},
                {"tradeofferid_countered", tradeofferidCountered.ToString()}
            };
            return JsonConvert.DeserializeObject<SendOfferResponse>(_web.Fetch(url, "POST", data, container, false,
                "https://steamcommunity.com/tradeoffer/" + tradeofferidCountered + "/").ReadStream());
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
                JsonConvert.DeserializeObject<GetTradeOffersSummaryBaseResponse>(_web.Fetch(url, "GET", data).ReadStream()).Response;
        }
    }
}