using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class RgDescription
    {
        [JsonProperty("appid")]
        public string AppId { get; set; }
        [JsonProperty("classid")]
        public string ClassId { get; set; }
        [JsonProperty("instanceid")]
        public string InstanceId { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("icon_drag_url")]
        public string IconDragUrl { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("market_hash_name")]
        public string MarketHashName { get; set; }
        [JsonProperty("market_name")]
        public string MarketName { get; set; }
        [JsonProperty("name_color")]
        public string NameColor { get; set; }
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("tradable")]
        public int Tradable { get; set; }
        [JsonProperty("marketable")]
        public int Marketable { get; set; }
        [JsonProperty("commodity")]
        public int Commodity { get; set; }
        [JsonProperty("market_tradable_restriction")]
        public string MarketTradableRestriction { get; set; }
        [JsonProperty("descriptions")]
        public List<Description> Descriptions { get; set; }
        [JsonProperty("owner_descriptions")]
        public string OwnerDescriptions { get; set; }
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}

