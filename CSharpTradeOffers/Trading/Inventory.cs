using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class Inventory
    {
        private readonly ulong _steamId;
        private readonly Web _web = new Web(new SteamRequestHandler());
        

        /// <summary>
        /// Class constructor, automatically initializes inventory.
        /// </summary>
        /// <param name="steamId">steamId64 of the inventory.</param>
        /// <param name="appId">App ID of the inventory.</param>
        public Inventory(ulong steamId, uint appId)
        {
            _steamId = steamId;
            InitializeInventory(appId);
        }

        /// <summary>
        /// String is the ClassId linking to an Item object.
        /// </summary>
        public Dictionary<string, Item> Items = new Dictionary<string, Item>();

        /// <summary>
        /// Returns the number of assets in the inventory.
        /// </summary>
        /// <returns>A ulong representing the number of assets.</returns>
        public ulong AssetCount()
        {
            ulong count = 0;
            foreach (Item value in from value in Items.Values from rgInventoryItem in value.Items select value)
                count++;
            return count;
        }

        /// <summary>
        /// Requests the inventory for the specified steamId and appId.
        /// </summary>
        /// <param name="steamId">steamId64 of the inventory to request.</param>
        /// <param name="appId">App ID of the inventory to request.</param>
        /// <returns>Returns a dynamic JSON object of the inventory to request.</returns>
        private dynamic RequestInventory(uint appId)
        {
            string url = "https://steamcommunity.com/profiles/" + _steamId + "/inventory/json/" + appId + "/2/";
            return JsonConvert.DeserializeObject<dynamic>(_web.Fetch(url, "GET", null, null, false));
        }

        /// <summary>
        /// Initializes the inventory for the specified steamId and appId.
        /// </summary>
        /// <param name="steamId">steamId64 of the inventory to request.</param>
        /// <param name="appId">App ID of the inventory to request.</param>
        private void InitializeInventory(uint appId)
        {
            dynamic inventoryJson = RequestInventory(appId);

            if (inventoryJson.success == null)
                throw new InventoryException("Inventory request was not successful. 'success' field was null.");
            if (inventoryJson.success != true && inventoryJson.Error != null)
                throw new InventoryException(
                    "Inventory request was not successful. 'success' field was false. Error Message: " +
                    inventoryJson.Error.ToString());
            if (inventoryJson.success != true)
                throw new InventoryException(
                    "Inventory request was not successsful. 'success' field was false. Likely cause: No items in inventory.");

            dynamic rgInventory = inventoryJson.rgInventory;
            dynamic rgDescriptions = inventoryJson.rgDescriptions;

            foreach (dynamic obj in rgDescriptions)
            {
                dynamic descriptionObj = JsonConvert.DeserializeObject<dynamic>(obj.Value.ToString());
                try
                {
                    var description = new Item
                    {
                        AppId = descriptionObj.appid,
                        ClassId = descriptionObj.classid,
                        MarketHashName = descriptionObj.market_hash_name,
                        Commodity = descriptionObj.commodity,
                        Marketable = descriptionObj.marketable,
                        Tradable = descriptionObj.tradable,
                        Worth =
                            ItemWorth(Convert.ToInt32(descriptionObj.tradable),
                                Convert.ToUInt32(descriptionObj.appid),
                                descriptionObj.market_hash_name.ToString())
                    };

                    if (!Items.ContainsKey(description.ClassId))
                        Items.Add(description.ClassId, description);
                }
                catch (NullReferenceException)
                {
                    //I can't remember why, but it prevents bad things from happening.
                }
            }

            foreach (dynamic obj in rgInventory)
            {
                dynamic item = JsonConvert.DeserializeObject<dynamic>(obj.Value.ToString());

                var inventoryItem = new RgInventoryItem
                {
                    Amount = item.amount,
                    ClassId = item.classid,
                    Id = item.id,
                    InstanceId = item.instanceid,
                    Pos = item.pos
                };
                Items[inventoryItem.ClassId.ToString()].Items.Add(inventoryItem);
            }
        }

        /// <summary>
        /// Requests decimal worth of an Item.
        /// </summary>
        /// <param name="item">An Item object to get the value of.</param>
        /// <returns>A decimal worth in USD.</returns>
        public decimal ItemWorth(Item item)
        {
            if (item.Tradable != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(item.AppId), item.MarketHashName);
            return Convert.ToDecimal(mv.MedianPrice.Substring(1)); //skips $ symbol
        }
        
        /// <param name="tradable"></param>
        /// <param name="appid"></param>
        /// <param name="marketHashName"></param>
        /// <returns></returns>
        public decimal ItemWorth(bool tradable, uint appid, string marketHashName)
        {
            if (tradable.IntValue() != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), marketHashName);
            return Convert.ToDecimal(mv.MedianPrice.Substring(1));
        }

        /// <param name="tradable"></param>
        /// <param name="appid"></param>
        /// <param name="marketHashName"></param>
        /// <returns></returns>
        public decimal ItemWorth(int tradable, uint appid, string marketHashName)
        {
            if (tradable != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), marketHashName);
            return Convert.ToDecimal(mv.MedianPrice.Substring(1));
        }

        /// <summary>
        /// Finds the first rgInventoryItem that is not in use.
        /// </summary>
        /// <param name="classid">ClassId of items to search.</param>
        /// <returns>An rgInventoryItem that is not marked in use.</returns>
        public RgInventoryItem FindAvailableAsset(string classid)
        {
            return Items[classid].Items.FirstOrDefault(item => !item.InUse);
        }

        /// <summary>
        /// Locates asset in Items[classid].items and marks it's inUse bool according to the inUse argument.
        /// </summary>
        /// <param name="asset">Asset to mark</param>
        /// <param name="inUse">Value to set</param>
        /// <returns>True if successful, false if not.</returns>
        public bool MarkAsset(CEconAsset asset, bool inUse)
        {
            foreach (RgInventoryItem item in Items[asset.ClassId].Items.Where(item => item.Id.ToString() == asset.AssetId))
            {
                item.InUse = inUse;
                return true;
            }
            return false;
        }
    }
}