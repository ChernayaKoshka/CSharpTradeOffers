using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTradeOffers.Configuration;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Handles Inventory-Related tasks for a specific account.
    /// </summary>
    public class InventoryHandler
    {
        private readonly ulong _steamId;

        private readonly string _apiKey;

        public InventoryHandler(ulong steamId, string apiKey)
        {
            _steamId = steamId;
            _apiKey = apiKey;
            Inventories = new Dictionary<uint, Inventory>();
        }

        public Dictionary<uint, Inventory> Inventories { get; }

        public void RefreshInventories(uint[] appids)
        {
            Inventories.Clear();
            foreach (uint appid in appids.Where(appid => !Inventories.ContainsKey(appid)))
                Inventories.Add(appid, new Inventory(_steamId, appid));
        }

        //predicate
        private static bool BeingUsed(rgInventory_Item rgInventoryItem)
        {
            return rgInventoryItem.inUse;
        }

        /// <summary>
        /// Locates an Item in the inventory.
        /// </summary>
        /// <param name="assetToFind">Specifies search params.</param>
        /// <returns>An item matching the params</returns>
        public Item FindUnusedItem(ItemValueHandler.ValuedWorth assetToFind)
        {
            Inventory inv = Inventories[assetToFind.appid];
            switch (assetToFind.typeid)
            {
                case 0: //exact
                    foreach (Item item in inv.Items.Values.Where(item => item.market_hash_name == assetToFind.typeobj).Where(item => !item.items.TrueForAll(BeingUsed)))
                        return item;
                    break;
                case 1: //contains
                    foreach (Item item in inv.Items.Values.Where(item => item.market_hash_name.ToLower().Contains(assetToFind.typeobj)).Where(item => !item.items.TrueForAll(BeingUsed)))
                        return item;
                    break;
                case 2: //startswirth
                    foreach (Item item in inv.Items.Values.Where(item => item.market_hash_name.ToLower().StartsWith(assetToFind.typeobj)).Where(item => !item.items.TrueForAll(BeingUsed)))
                        return item;
                    break;
                case 3: //classid
                    foreach (
                        Item item in
                            inv.Items.Values.Where(
                                item => item.classid == assetToFind.typeobj && item.items.TrueForAll(BeingUsed)))
                        return item;
                    return null;
                case 4: //tags
                    var handler = new SteamEconomyHandler(_apiKey);
                    foreach (var item in inv.Items.Values)
                    {
                        Dictionary<string, string> classid = new Dictionary<string, string>
                        {
                            {item.classid, null}
                        };
                        AssetClassInfo info = handler.GetAssetClassInfo(Convert.ToUInt32(item.appid), classid);
                        foreach (var tag in info.tags.Values)
                        {
                            if (tag.name == assetToFind.typeobj)
                            {
                                if (!item.items.TrueForAll(BeingUsed)) return item;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    throw new Exception("Unknown TypeId!");
            }
            return null;
        }

        /// <param name="marketHashName">Name to search</param>
        /// <param name="appid">Appid of the inventory to search</param>
        /// <returns>A list of items whose market_hash_name contains marketHashName</returns>
        public List<Item> FindUnusedItems(string marketHashName, uint appid)
        {
            Inventory inv = Inventories[appid];
            List<Item> items = inv.Items.Values.Where(item => item.market_hash_name.ToLower().Contains(marketHashName) && !item.items.TrueForAll(BeingUsed)).ToList();
            return items;
        }

        /// <summary>
        /// Locates an Item in the inventory.
        /// </summary>
        /// <param name="assetToFind">Specifies search params.</param>
        /// <param name="inv">Inventory to search.providing the </param>
        /// <returns></returns>
        public Item FindUnusedItem(CEconAsset assetToFind, Inventory inv)
        {
            //Inventory inv = Inventories[Convert.ToUInt32(assetToFind.appid)];
            return inv.Items[assetToFind.classid];
        }

        /// <summary>
        /// Locates an Item in the inventory.
        /// </summary>
        /// <param name="assetToFind">Specifies search params.</param>
        /// <returns></returns>
        public Item FindFirstItem(CEconAsset assetToFind)
        {
            Inventory inv = Inventories[Convert.ToUInt32(assetToFind.appid)];
            return inv.Items[assetToFind.classid];
        }

        /// <summary>
        /// Marks the inUse bool of the assets specified in the trade offer.
        /// </summary>
        /// <param name="offer">TradeOffer to mark.</param>
        /// <param name="inUse">Bool to set.</param>
        public void MarkMyAssets(CEconTradeOffer offer, bool inUse)
        {
            foreach (CEconAsset asset in offer.items_to_give)
                Inventories[Convert.ToUInt32(asset.appid)].MarkAsset(asset, inUse);
        }

        /// <summary>
        /// Marks the inUse bool of the assets specified in the trade offer.
        /// </summary>
        /// <param name="offer">TradeOffer to mark.</param>
        /// <param name="inUse">Bool to set.</param>
        public void MarkMyAssets(TradeOffer offer, bool inUse)
        {
            foreach (var asset in offer.me.assets)
                Inventories[Convert.ToUInt32(asset.appid)].MarkAsset(asset, inUse);
        } 

        /// <summary>
        /// Requests decimal worth of an Item.
        /// </summary>
        /// <param name="item">An Item object to get the value of.</param>
        /// <returns>A decimal worth in USD.</returns>
        public decimal ItemWorth(Item item)
        {
            if (item.tradable != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(item.appid), item.market_hash_name);
            return Convert.ToDecimal(mv.median_price.Substring(1));
        }

        /// <param name="marketable"></param>
        /// <param name="appid"></param>
        /// <param name="marketHashName"></param>
        /// <returns></returns>
        public decimal ItemWorth(bool marketable, uint appid, string marketHashName)
        {
            if (marketable.IntValue() != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), marketHashName);
            return Convert.ToDecimal(mv.median_price.Substring(1));
        }


        /// <param name="marketable"></param>
        /// <param name="appid"></param>
        /// <param name="marketHashName"></param>
        /// <returns></returns>
        public decimal ItemWorth(int marketable, uint appid, string marketHashName)
        {
            if (marketable != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), marketHashName);
            return Convert.ToDecimal(mv.median_price.Substring(1));
        }
    }
}
