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
        /// RgDescription of an item, kind of like AssetClassInfo but with less information.
        /// </summary>
        public RgDescription Description { get; set; }

        /// <summary>
        /// List of rgInventoryItem, this should be used for whenever you need to interact with a specific item rather than all of them.
        /// </summary>
        public List<RgInventoryItem> Items { get; } = new List<RgInventoryItem>();
    }
}