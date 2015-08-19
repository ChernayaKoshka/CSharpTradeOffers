using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpTradeOffers.Trading
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Handles Steam Economy related tasks, like retrieving class info
    /// </summary>
    public class ISteamEconomyHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/ISteamEconomy/";

        /// <summary>
        /// Gets the asset class info of an item, must provide ClassID/InstanceID in IDs
        /// </summary>
        /// <param name="apiKey">Your Steam API key</param>
        /// <param name="appid">Uint32 number that represents the game to retrieve item data from.</param>
        /// <param name="IDs">Dictionary MUST contain ClassID/InstanceID of item.</param>
        /// <returns></returns>
        public AssetClassInfo GetAssetClassInfo(string apiKey, uint appid, Dictionary<string, string> IDs)
        {
            const string url = BaseUrl + "GetAssetClassInfo/v0001/";
            var data = new Dictionary<string, string>
            {
                {"key", apiKey},
                {"appid", appid.ToString()},
                {"class_count", IDs.Count.ToString()}
            };
            int currentClass = 0;
            foreach (var key in IDs) //make only request per appid at a time
            {
                data.Add("classid" + currentClass, key.Key);
                data.Add("instanceid" + currentClass, key.Value);
                currentClass++;
            }

            var desrDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Web.Fetch(url, "GET", data, null, false));

            desrDictionary.Remove("success");

            return JsonConvert.DeserializeObject<AssetClassInfo>(desrDictionary.Values.First().ToString());
        }

        #region old
        /*
        /// <summary>
        /// Partially converts the dynamic GetAssetClassInfo into an AssetClassInfo object.
        /// Does not convert filter_data because its complicated and I'm lazy.
        /// Very poorly done, possibly optimize/fix?
        /// </summary>
        /// <param name="dynamicinfo">The return variable from GetAssetClassInfo</param>
        /// <returns>An AssetClassInfo object.</returns>
        // ReSharper disable once FunctionComplexityOverflow
        public AssetClassInfo ToAssetClassInfo(dynamic dynamicinfo)
        {
            Dictionary<string, dynamic> desrDictionary =
                JsonConvert.DeserializeObject <Dictionary<string, dynamic>>(dynamicinfo.ToString());
            desrDictionary.Remove("success");
            return JsonConvert.DeserializeObject<AssetClassInfo>(desrDictionary.Values.First().ToString());

            //testinfo test = JsonConvert.DeserializeObject<testinfo>(desrDictionary.Values.First().ToString());

            //var info = new AssetClassInfo();
            //var classinfo = (JObject) dynamicinfo;
            //foreach (var root in classinfo.First.Children())
            //{
            //    info.icon_url = root["icon_url"].ToString();
            //    info.icon_url_large = root["icon_url_large"].ToString();
            //    info.icon_drag_url = root["icon_drag_url"].ToString();
            //    info.name = root["name"].ToString();
            //    info.market_hash_name = root["market_hash_name"].ToString();
            //    info.market_name = root["market_name"].ToString();
            //    info.name_color = root["name_color"].ToString();
            //    info.background_color = root["background_color"].ToString();
            //    info.type = root["type"].ToString();
            //    info.tradable = root["tradable"].ToString();
            //    info.marketable = root["marketable"].ToString();
            //    info.commodity = root["commodity"].ToString();
            //    info.market_tradable_restriction = root["market_tradable_restriction"].ToString();
            //    info.fraudwarnings = root["fraudwarnings"].ToString();
            //    foreach (var desc in root["descriptions"].Children())
            //    {
            //        var description = new Description();
            //        if (desc.First["type"] != null)
            //            description.type = desc.First["type"].ToString();
            //        if (desc.First["value"] != null)
            //            description.value = desc.First["value"].ToString();
            //        if (desc.First["color"] != null)
            //            description.color = desc.First["color"].ToString();
            //        if (desc.First["app_data"] != null)
            //            description.app_data = desc.First["app_data"].ToString();
            //
            //        info.descriptions.Add(description);
            //    }
            //    foreach (var act in root["actions"].Children())
            //    {
            //        var action = new Action();
            //        action.name = act.First["name"].ToString();
            //        action.link = act.First["link"].ToString();
            //        info.actions.Add(action);
            //    }
            //    foreach (var t in root["tags"].Children())
            //    {
            //        var tag = new Tag();
            //        if (t.First["internal_name"] != null)
            //            tag.internal_name = t.First["internal_name"].ToString();
            //        if (t.First["name"] != null)
            //            tag.name = t.First["name"].ToString();
            //        if (t.First["category"] != null)
            //            tag.category = t.First["category"].ToString();
            //        if (t.First["color"] != null)
            //            tag.color = t.First["color"].ToString();
            //        if (t.First["category_name"] != null)
            //            tag.category_name = t.First["category_name"].ToString();
            //        info.tags.Add(tag);
            //    }
            //
            //    if (root["app_data"]["def_index"] != null)
            //        info.appdata2.def_index = root["app_data"]["def_index"].ToString();
            //    if (root["app_data"]["quality"] != null)
            //        info.appdata2.quality = root["app_data"]["quality"].ToString();
            //    if (root["app_data"]["slot"] != null)
            //        info.appdata2.slot = root["app_data"]["slot"].ToString();
            //    if (root["app_data"]["set_bundle_def_index"] != null)
            //        info.appdata2.set_bundle_def_index = root["app_data"]["set_bundle_def_index"].ToString();
            //
            //    if (root["app_data"]["containing_bundles"] != null)
            //        foreach (var bundles in root["app_data"]["containing_bundles"].Children())
            //        {
            //            info.appdata2.containing_bundles.Add(bundles.ToString());
            //        }
            //    //foreach(var filterdata in root["app_data"]["filter_data"].Children())
            //    //{
            //    //    foreach(var element_ids in filterdata.Children())
            //    //    {
            //    //        info.appdata2.filter_data.Add();
            //    //    }
            //    //}
            //    info.classid = root["classid"].ToString();
            //}
            //return info;
        }
        */
        #endregion
    }
}