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

        //Fuuuuuuuuck thiiiiiiiiis
        [Obsolete("Do not set this, it is a workaround!")]
        [JsonProperty("contextid")]
        public string ContextIdDontSet { get; set; }

        [Obsolete("Do not set this, it is a workaround!")]
        [JsonProperty("assetid")]
        public string AssetIdDontSet { get; set; }

        //end fuuuuuuuuuuck thiiiiiiis
        
        [JsonIgnore]
        [JsonProperty("contextid")]
        public long ContextId
        {
            get { return Convert.ToInt32(AssetIdDontSet); }
            set
            {
                ContextIdDontSet = value.ToString();

            }
        }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonIgnore]
        [JsonProperty("assetid")]
        public long AssetId
        {
            get { return Convert.ToInt32(AssetIdDontSet); }
            set
            {
                AssetIdDontSet = value.ToString();
            }
        }


        [JsonProperty("classid")]
        public long ClassId { get; set; }

        [JsonProperty("instanceid")]
        public long InstanceId { get; set; }

        [JsonProperty("missing")]
        public bool Missing { get; set; }

        /// <param name="apiKey"></param>
        /// <returns></returns>
        public string GetMarketHashName(string apiKey)
        {
            var handler = new SteamEconomyHandler(apiKey);
            var data = new Dictionary<string, string> {{ClassId.ToString(), InstanceId.ToString()}};
            AssetClassInfo info = handler.GetAssetClassInfo(Convert.ToUInt32(AppId), data);
            return info.MarketHashName;
        }
    }
}