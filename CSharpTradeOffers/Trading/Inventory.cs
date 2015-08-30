using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class Inventory
    {
        private readonly ulong _steamId;
        #region old, remove later + fix
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


        /// <param name="tradable"></param>
        /// <param name="appid"></param>
        /// <param name="marketHashName"></param>
        /// <returns></returns>
        public decimal ItemWorth(bool tradable, uint appid, string marketHashName)
        {
            if (tradable.IntValue() != 1) return 0.0m;
            var handler = new MarketHandler();
            MarketValue mv = handler.GetPriceOverview(Convert.ToUInt32(appid), marketHashName);
            return Convert.ToDecimal(mv.median_price.Substring(1));
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
            return Convert.ToDecimal(mv.median_price.Substring(1));
        }
        #endregion

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
        /// Requests the inventory for the specified steamId and appId.
        /// </summary>
        /// <param name="steamId">steamId64 of the inventory to request.</param>
        /// <param name="appId">App ID of the inventory to request.</param>
        /// <returns>Returns a dynamic JSON object of the inventory to request.</returns>
        private dynamic RequestInventory(uint appId)
        {
            string url = "https://steamcommunity.com/profiles/" + _steamId + "/inventory/json/" + appId + "/2/";
            return JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", null, null, false));
        }

        /// <summary>
        /// Initializes the inventory for the specified steamId and appId.
        /// </summary>
        /// <param name="steamId">steamId64 of the inventory to request.</param>
        /// <param name="appId">App ID of the inventory to request.</param>
        private void InitializeInventory(uint appId)
        {
            dynamic inventoryJson = RequestInventory(appId);

            if(inventoryJson.success == null) throw new InventoryException("Inventory request was not successful. 'success' field was null.");
            if (inventoryJson.success != true && inventoryJson.Error != null) throw new InventoryException("Inventory request was not successful. 'success' field was false. Error Message: " + inventoryJson.Error.ToString());
            if(inventoryJson.success != true) throw new InventoryException("Inventory request was not successsful. 'success' field was false. Likely cause: No items in inventory.");

            dynamic rgInventory = inventoryJson.rgInventory;
            dynamic rgDescriptions = inventoryJson.rgDescriptions;

            foreach (dynamic obj in rgDescriptions)
            {
                dynamic descriptionObj = JsonConvert.DeserializeObject<dynamic>(obj.Value.ToString());
                try
                {
                    var description = new Item
                    {
                        appid = descriptionObj.appid,
                        classid = descriptionObj.classid,
                        market_hash_name = descriptionObj.market_hash_name,
                        commodity = descriptionObj.commodity,
                        marketable = descriptionObj.marketable,
                        tradable = descriptionObj.tradable,
                        worth =
                            ItemWorth(Convert.ToInt32(descriptionObj.tradable),
                                Convert.ToUInt32(descriptionObj.appid),
                                descriptionObj.market_hash_name.ToString())
                    };

                    if (!Items.ContainsKey(description.classid))
                        Items.Add(description.classid, description);
                }
                catch (NullReferenceException)
                {
                    //I can't remember why, but it prevents bad things from happening.
                }
            }

            foreach (dynamic obj in rgInventory)
            {
                dynamic item = JsonConvert.DeserializeObject<dynamic>(obj.Value.ToString());

                var inventoryItem = new rgInventory_Item
                {
                    amount = item.amount,
                    classid = item.classid,
                    id = item.id,
                    instanceid = item.instanceid,
                    pos = item.pos
                };
                Items[inventoryItem.classid.ToString()].items.Add(inventoryItem);
            }
        }

        /// <summary>
        /// Finds the first rgInventory_Item that is not in use.
        /// </summary>
        /// <param name="classid">ClassId of items to search.</param>
        /// <returns>An rgInventory_Item that is not marked in use.</returns>
        public rgInventory_Item FindAvailableAsset(string classid)
        {
            return Items[classid].items.FirstOrDefault(item => !item.inUse);
        }

        /// <summary>
        /// Locates asset in Items[classid].items and marks it's inUse bool according to the inUse argument.
        /// </summary>
        /// <param name="asset">Asset to mark</param>
        /// <param name="inUse">Value to set</param>
        /// <returns>True if successful, false if not.</returns>
        public bool MarkAsset(CEconAsset asset, bool inUse)
        {
            foreach (rgInventory_Item item in Items[asset.classid].items)
            {
                if (item.id.ToString() != asset.assetid) continue;
                item.inUse = inUse;
                return true;
            }
            return false;
        }
    }
}