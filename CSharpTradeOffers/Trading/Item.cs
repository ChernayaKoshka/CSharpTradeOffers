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
    }
}