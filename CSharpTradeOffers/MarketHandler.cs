using System.Collections.Generic;
using System.Net;
using CSharpTradeOffers.Trading;
using Newtonsoft.Json;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Handles market related tasks.
    /// </summary>
    public class MarketHandler
    {
        const string BaseUrl = "https://steamcommunity.com/market/";
        
        /// <summary>
        /// Sets the container to contain a MarketEligibility cookie. Required before trading.
        /// </summary>
        /// <param name="steamId">The SteamId of the bot.</param>
        /// <param name="container">The bot CookieContainer</param>
        public void EligibilityCheck(ulong steamId, CookieContainer container)
        {
            const string url = BaseUrl + "eligibilitycheck/";
            var data = new Dictionary<string, string> { { "goto", "/profiles/" + steamId + "/tradeoffers/" } };
            Web.Fetch(url, "GET", data, container, false);
        }

        /// <summary>
        /// Gets the price overview of an item.
        /// </summary>
        /// <param name="appId">The appId of the item.</param>
        /// <param name="marketHashName">The market_hash_name of the item</param>
        /// <param name="country">Country to check in. (ISO)</param>
        /// <param name="currency">Currency code, I forget what. 1 = US $</param>
        /// <returns>A MarketValue object containing the data.</returns>
        public MarketValue GetPriceOverview(uint appId, string marketHashName, string country = "US", string currency = "1")
        {
            const string url = BaseUrl + "priceoverview/";
            var data = new Dictionary<string, string>
            {
                {"country", country},
                {"currency", currency},
                {"appid", appId.ToString()},
                {"market_hash_name", marketHashName}
            };
            return JsonConvert.DeserializeObject<MarketValue>(Web.Fetch(url, "GET", data, null, false));
        }
    }
}