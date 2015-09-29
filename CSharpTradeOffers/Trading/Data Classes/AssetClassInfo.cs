using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class AssetClassInfo
    {
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
        public string Tradable { get; set; }

        [JsonProperty("marketable")]
        public string Marketable { get; set; }

        [JsonProperty("commodity")]
        public string Commodity { get; set; }

        [JsonProperty("market_tradable_restriction")]
        public string MarketTradableRestriction { get; set; }

        [JsonProperty("fraudwarnings")]
        public Dictionary<string, dynamic> FraudWarnings { get; set; }

        [JsonProperty("descriptions")]
        public Dictionary<string, Description> Descriptions { get; set; }

        [JsonProperty("owner_descriptions")]
        public string OwnerDescriptions { get; set; }

        [JsonProperty("actions")]
        public Dictionary<string, Action> Actions { get; set; }

        [JsonProperty("market_actions")]
        public Dictionary<string, dynamic> MarketActions { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, Tag> Tags { get; set; }

        [JsonProperty("classid")]
        public string ClassId { get; set; }

        [JsonProperty("instanceid")]
        public string InstanceId { get; set; }
    }

    [JsonObject(Title = "Element_IDs")]
    public class Elements
    {
        [JsonProperty("ids")]
        public List<string> Ids { get; } = new List<string>();
    }

    public class Description
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("app_data")]
        public DescriptionAppData AppData { get; set; }

        [JsonProperty("is_itemset_name")]
        public string IsItemsetName { get; set; }

        [JsonProperty("def_index")]
        public string DefIndex { get; set; }
    }

    [JsonObject(Title = "app_data")]
    public class DescriptionAppData
    {
        [JsonProperty("def_index")]
        public int DefIndex { get; set; }
        [JsonProperty("is_itemset_name")]
        public int IsItemSetName { get; set; }
        [JsonProperty("limited")]
        public int Limited { get; set; }
    }

    public class AppData2
    {
        [JsonProperty("def_index")]
        public string DefIndex { get; set; }

        [JsonProperty("quality")]
        public string Quality { get; set; }

        [JsonProperty("slot")]
        public string Slot { get; set; }

        [JsonProperty("set_bundle_def_index")]
        public string SetBundleDefIndex { get; set; }

        [JsonProperty("containing_bundles")]
        public List<string> ContainingBundles { get; } = new List<string>();

        [JsonProperty("filter_data")]
        public List<FilterData> Data { get; } = new List<FilterData>();

        [JsonProperty("player_class_ids")]
        public List<string> PlayerClassIds { get; } = new List<string>();
    }

    [JsonObject(Title = "Filter_Data")]
    public class FilterData
    {
        [JsonProperty("element_ids")]
        public List<Elements> ElementIds { get; } = new List<Elements>();
    }
}