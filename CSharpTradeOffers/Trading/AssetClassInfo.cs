using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class AssetClassInfo
    {
        public string icon_url { get; set; }
        public string icon_url_large { get; set; }
        public string icon_drag_url { get; set; }
        public string name { get; set; }
        public string market_hash_name { get; set; }
        public string market_name { get; set; }
        public string name_color { get; set; }
        public string background_color { get; set; }
        public string type { get; set; }
        public string tradable { get; set; }
        public string marketable { get; set; }
        public string commodity { get; set; }
        public string market_tradable_restriction { get; set; }
        public Dictionary<string, dynamic> fraudwarnings { get; set; }
        public Dictionary<string, Description> descriptions { get; set; }
        public string owner_descriptions { get; set; }
        public Dictionary<string, Action> actions { get; set; }
        public Dictionary<string, dynamic> market_actions { get; set; }
        public Dictionary<string, Tag> tags { get; set; }
        public string classid { get; set; }
        public string instanceid { get; set; }
    }

    public class Element_IDs
    {

        public List<string> ids = new List<string>();
    }
    public class Description
    {

        public string type { get; set; }

        public string value { get; set; }

        public string color { get; set; }

        public string app_data { get; set; }

        public string is_itemset_name { get; set; }

        public string def_index { get; set; }
    }
    public class AppData2
    {

        public string def_index { get; set; }

        public string quality { get; set; }

        public string slot { get; set; }

        public string set_bundle_def_index { get; set; }

        public List<string> containing_bundles = new List<string>();

        public List<Filter_Data> filter_data = new List<Filter_Data>();

        public List<string> player_class_ids = new List<string>();
    }
    public class Filter_Data
    {

        public List<Element_IDs> element_ids = new List<Element_IDs>();
    }
}