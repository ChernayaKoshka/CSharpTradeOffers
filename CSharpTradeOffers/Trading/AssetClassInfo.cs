using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    #region AssetClassInfo

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    /*public class AssetClassInfo
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_url { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_url_large { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_drag_url { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_hash_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name_color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string background_color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string tradable { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string marketable { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string commodity { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_tradable_restriction { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_marketable_restriction { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string fraudwarnings { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Description> descriptions = new List<Description>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Action> actions = new List<Action>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Tag> tags = new List<Tag>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public AppData2 appdata2 = new AppData2();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string classid { get; set; }
    }*/

    #endregion

#pragma warning disable 1591
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
#pragma warning restore 1591

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class Element_IDs
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<string> ids = new List<string>();
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class Description
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string app_data { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string is_itemset_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string def_index { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class AppData2
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string def_index { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string quality { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string slot { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string set_bundle_def_index { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<string> containing_bundles = new List<string>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Filter_Data> filter_data = new List<Filter_Data>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<string> player_class_ids = new List<string>();
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class Filter_Data
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Element_IDs> element_ids = new List<Element_IDs>();
    }
}