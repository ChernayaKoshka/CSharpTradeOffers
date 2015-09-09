using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "ItemsToReceive")]
    public class CEconAsset
    {
        [JsonProperty("appid")]
        public string AppId { get; set; }

        [JsonProperty("contextid")]
        public string ContextId { get; set; }

        [JsonProperty("assetid")]
        public string AssetId { get; set; }

        [JsonProperty("classid")]
        public string ClassId { get; set; }

        [JsonProperty("instanceid")]
        public string InstanceId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("missing")]
        public bool Missing { get; set; }

        /// <param name="apiKey"></param>
        /// <returns></returns>
        public string GetMarketHashName(string apiKey)
        {
            var handler = new SteamEconomyHandler(apiKey);
            var data = new Dictionary<string, string> { { this.ClassId, this.InstanceId } };
            AssetClassInfo info = handler.GetAssetClassInfo(Convert.ToUInt32(this.AppId), data);
            return info.MarketHashName;
        }
    }
}