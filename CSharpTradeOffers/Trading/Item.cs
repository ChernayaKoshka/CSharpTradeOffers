using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// An inventory item type class.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// AppId of item
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// ClassId of item
        /// </summary>
        public string classid { get; set; }
        /// <summary>
        /// Market hash name of item, used for searching price as well as general matching.
        /// </summary>
        public string market_hash_name { get; set; }
        /// <summary>
        /// 0 = not tradable, 1 = tradable.
        /// </summary>
        public int tradable { get; set; }
        /// <summary>
        /// 0 = not marketable, 1 = marketable
        /// </summary>
        public int marketable { get; set; }
        /// <summary>
        /// 0 = not commodity, 1 = commodity
        /// </summary>
        public int commodity { get; set; }
        /// <summary>
        /// List of rgInventory_Item, this should be used for whenever you need to interact with a specific item rather than all of them.
        /// </summary>
        public List<rgInventory_Item> items = new List<rgInventory_Item>();
        /// <summary>
        /// Market worth of the item.
        /// </summary>
        [JsonIgnore]
        public decimal worth { get; set; }

        #region old
        /*public decimal Worth()
        {
            if (tradable != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), market_hash_name);
            return Convert.ToDecimal(mv.median_price.Substring(0, 1));
        }*/
        /*/// <summary>
        /// Gets the market_hash_name of the item.
        /// </summary>
        /// <param name="apiKey">The bot's API key.</param>
        /// <returns>The market_hash_name of the item.</returns>
        public string GetMarketHashName(string apiKey)
        {
            var handler = new ISteamEconomyHandler();
            var data = new Dictionary<string, string> {{classid, "0"}};
            AssetClassInfo info =
                handler.ToAssetClassInfo(handler.GetAssetClassInfo(apiKey, Convert.ToUInt32(appid), data).result);
            return info.market_hash_name;
        }*/
        #endregion
    }
}