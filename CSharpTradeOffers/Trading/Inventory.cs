using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTradeOffers.Web;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class Inventory
    {
        private readonly ulong _steamId;

        private readonly Web.Web _web = new Web.Web(new SteamWebRequestHandler());
        
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
        public Dictionary<long, Item> Items { get; } = new Dictionary<long, Item>();

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
            return JsonConvert.DeserializeObject<dynamic>(_web.Fetch(url, "GET", null, null, false).ReadStream());
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
            Dictionary<string, RgDescription> deserializedDescriptions =
                JsonConvert.DeserializeObject<Dictionary<string, RgDescription>>(rgDescriptions.ToString());

            foreach (RgDescription rgDescription in deserializedDescriptions.Values)
            {
                try
                {
                    var description = new Item {Description = rgDescription};

                    if (!Items.ContainsKey(rgDescription.ClassId))
                        Items.Add(rgDescription.ClassId, description);
                }
                catch (NullReferenceException)
                {
                    //I can't remember why, but it prevents bad things from happening.
                }
            }

            //later implement RgInventory like I did with RgDescription
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
                Items[inventoryItem.ClassId].Items.Add(inventoryItem);
            }
        }

        //predicate
        private static bool BeingUsed(RgInventoryItem rgInventoryItem)
        {
            return rgInventoryItem.InUse;
        }

        /// <param name="marketHashName">Name to search</param>
        /// <param name="appid">Appid of the inventory to search</param>
        /// <returns>A list of items whose market_hash_name contains marketHashName</returns>
        public List<Item> FindUnusedItems(string marketHashName)
        {
            List<Item> items = Items.Values.Where(item => item.Description.MarketHashName.ToLower().Contains(marketHashName) && !item.Items.TrueForAll(BeingUsed)).ToList();
            return items;
        }

        /// <summary>
        /// Locates an Item in the inventory.
        /// </summary>
        /// <param name="assetToFind">Specifies search params.</param>
        /// <returns></returns>
        public Item FindFirstItem(CEconAsset assetToFind)
        {
            return Items[assetToFind.ClassId];
        }

        /// <summary>
        /// Finds the first rgInventoryItem that is not in use.
        /// </summary>
        /// <param name="classId">ClassId of items to search.</param>
        /// <returns>An rgInventoryItem that is not marked in use.</returns>
        public RgInventoryItem FindAvailableAsset(long classId)
        {
            return Items[classId].Items.FirstOrDefault(item => !item.InUse);
        }

        /// <summary>
        /// Locates asset in Items[classid].items and marks it's inUse bool according to the inUse argument.
        /// </summary>
        /// <param name="asset">Asset to mark</param>
        /// <param name="inUse">Value to set</param>
        /// <returns>True if successful, false if not.</returns>
        public bool MarkAsset(CEconAsset asset, bool inUse)
        {
            foreach (RgInventoryItem item in Items[asset.ClassId].Items.Where(item => item.Id == asset.AssetId))
            {
                item.InUse = inUse;
                return true;
            }
            return false;
        }
    }
}