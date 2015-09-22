using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    /// <summary>
    /// Description object contained in the dynamic JSON returned by RequestInventory.
    /// </summary>
    [JsonObject(Title = "rgDescription")]
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

        [JsonProperty("icon_url_large")]
        public string IconUrlLarge { get; set; }

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

        [JsonProperty("commodity")] // ToDo: spelled wrong? Needs to be checked
        public int Commodity { get; set; }

        [JsonProperty("market_tradable_restriction")]
        public string MarketTradableRestriction { get; set; }

        [JsonProperty("market_marketable_restriction")]
        public string MarketMarketableRestriction { get; set; }

        [JsonProperty("descriptions")]
        public List<Description> Descriptions { get; } = new List<Description>();

        [JsonProperty("actions")]
        public List<Action> Actions { get; } = new List<Action>();

        [JsonProperty("tags")]
        public List<Tag> Tags { get; } = new List<Tag>();

        [JsonProperty("app_data")]
        public AppData AppData { get; set; }
    }
}