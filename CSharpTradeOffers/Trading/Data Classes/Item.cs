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
        [JsonProperty("appid")]
        public string AppId { get; set; }
        /// <summary>
        /// ClassId of item
        /// </summary>
        [JsonProperty("classid")]
        public string ClassId { get; set; }
        /// <summary>
        /// Market hash name of item, used for searching price as well as general matching.
        /// </summary>
        [JsonProperty("market_hash_name")]
        public string MarketHashName { get; set; }
        /// <summary>
        /// 0 = not tradable, 1 = tradable.
        /// </summary>
        [JsonProperty("tradable")]
        public int Tradable { get; set; }
        /// <summary>
        /// 0 = not marketable, 1 = marketable
        /// </summary>
        [JsonProperty("marketable")]
        public int Marketable { get; set; }
        /// <summary>
        /// 0 = not commodity, 1 = commodity
        /// </summary>
        [JsonProperty("commodity")] 
        public int Commodity { get; set; } // ToDo: commodity spelled wrong!?

        /// <summary>
        /// List of rgInventoryItem, this should be used for whenever you need to interact with a specific item rather than all of them.
        /// </summary>
        [JsonProperty("items")]
        public List<RgInventoryItem> Items { get; } = new List<RgInventoryItem>();

        /// <summary>
        /// Market worth of the item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("worth")]
        public decimal Worth { get; set; }
    }
}