using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "ItemsToReceive")]
    public class CEconAsset
    {
        [JsonProperty("appid")]
        public uint AppId { get; set; }

        [JsonProperty("contextid")]
        public long ContextId { get; set; }

        [JsonProperty("assetid")]
        public long AssetId { get; set; }

        [JsonProperty("classid")]
        public long ClassId { get; set; }

        [JsonProperty("instanceid")]
        public long InstanceId { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("missing")]
        public bool Missing { get; set; }

        /// <param name="apiKey"></param>
        /// <returns></returns>
        public string GetMarketHashName(string apiKey)
        {
            var handler = new SteamEconomyHandler(apiKey);
            var data = new Dictionary<string, string> { { this.ClassId.ToString(), this.InstanceId.ToString() } };
            AssetClassInfo info = handler.GetAssetClassInfo(Convert.ToUInt32(this.AppId), data);
            return info.MarketHashName;
        }
    }
}