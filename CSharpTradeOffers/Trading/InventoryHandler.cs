using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Handles Inventory-Related tasks for a specific account.
    /// </summary>
    public class InventoryHandler
    {
        private readonly ulong _steamId;

        public InventoryHandler(ulong steamId)
        {
            _steamId = steamId;
            Inventories = new Dictionary<uint, Inventory>();
        }

        public Dictionary<uint, Inventory> Inventories { get; }

        public void RefreshInventories(uint[] appids)
        {
            Inventories.Clear();
            foreach (uint appid in appids.Where(appid => !Inventories.ContainsKey(appid)))
                Inventories.Add(appid, new Inventory(_steamId, appid));
        }

        /// <summary>
        /// Marks the inUse bool of the assets specified in the trade offer.
        /// </summary>
        /// <param name="offer">TradeOffer to mark.</param>
        /// <param name="inUse">Bool to set.</param>
        public void MarkMyAssets(CEconTradeOffer offer, bool inUse)
        {
            foreach (CEconAsset asset in offer.ItemsToGive)
                Inventories[Convert.ToUInt32(asset.AppId)].MarkAsset(asset, inUse);
        }

        /// <summary>
        /// Marks the inUse bool of the assets specified in the trade offer.
        /// </summary>
        /// <param name="offer">TradeOffer to mark.</param>
        /// <param name="inUse">Bool to set.</param>
        public void MarkMyAssets(TradeOffer offer, bool inUse)
        {
            foreach (var asset in offer.Me.Assets)
                Inventories[Convert.ToUInt32(asset.AppId)].MarkAsset(asset, inUse);
        } 
    }
}
