using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "ItemsToReceive")]
    public class CEconAsset
    {

        public string appid { get; set; }

        public string contextid { get; set; }

        public string assetid { get; set; }

        public string classid { get; set; }

        public string instanceid { get; set; }

        public string amount { get; set; }

        public bool missing { get; set; }

        /// <param name="apiKey"></param>
        /// <returns></returns>
        public string GetMarketHashName(string apiKey)
        {
            var _handler = new SteamEconomyHandler();
            var data = new Dictionary<string, string> { { classid, instanceid } };
            AssetClassInfo info = _handler.GetAssetClassInfo(apiKey, Convert.ToUInt32(appid), data);
            return info.market_hash_name;
        }
    }
}