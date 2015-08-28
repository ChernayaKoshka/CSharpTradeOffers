using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTradeOffers.Configuration;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Handles Inventory-Related tasks for a specific account.
    /// </summary>
    public class InventoryHandler
    {
        //private readonly Config _config;
        //private Account _account;
        private readonly ulong _steamId;
        private readonly string _apiKey;

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="account"></param>
        public InventoryHandler(ulong steamId, string apiKey)
        {
            _steamId = steamId;
            _apiKey = apiKey;
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public Dictionary<uint, Inventory> Inventories = new Dictionary<uint, Inventory>();

        //predicate
        private static bool BeingUsed(rgInventory_Item rgInventoryItem)
        {
            return rgInventoryItem.inUse;
        }

        /// <summary>
        /// Locates an Item in the inventory.
        /// </summary>
        /// <param name="assetToFind">Specifies search params.</param>
        /// <returns></returns>
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
                    var handler = new ISteamEconomyHandler();
                    foreach (var item in inv.Items.Values)
                    {
                        Dictionary<string, string> classid = new Dictionary<string, string>
                        {
                            {item.classid, null}
                        };
                        AssetClassInfo info = handler.GetAssetClassInfo(_apiKey, Convert.ToUInt32(item.appid), classid);
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="marketHashName"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
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
        public Item FindFirstItem(TradeConfig.ConfigAsset assetToFind)
        {
            Inventory inv = Inventories[assetToFind.AppId];
            switch (assetToFind.TypeId)
            {
                case 0:
                    foreach (Item item in inv.Items.Values)
                        if (item.market_hash_name == assetToFind.TypeObj)
                            return item;
                    break;
                case 1:
                    foreach (Item item in inv.Items.Values)
                        if (item.market_hash_name.ToLower().Contains(assetToFind.TypeObj))
                            return item;
                    break;
                case 2:
                    foreach (Item item in inv.Items.Values)
                        if (item.market_hash_name.ToLower().StartsWith(assetToFind.TypeObj))
                            return item;
                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:
                    //ISteamEconomyHandler handler = new ISteamEconomyHandler();
                    //foreach (Item item in inv.Items.Values)
                    //{
                    //    var classid = new Dictionary<string, string>
                    //    {
                    //        {item.classid, null}
                    //    };
                    //
                    //    AssetClassInfo info = handler.ToAssetClassInfo(
                    //        handler.GetAssetClassInfo(_config.Cfg.ApiKey, Convert.ToUInt32(item.appid),
                    //            classid).result);
                    //
                    //    if (info.tags.Any(tag => tag.name == assetToFind.TypeObj))
                    //    {
                    //        return item;
                    //    }
                    //}
                    break;
                default:
                    throw new Exception("Unknown TypeId!");
            }
            return null;
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

        ///// <summary>
        /// Marks the inUse bool of the specified asset.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="inUse"></param>
        //public void MarkMyAssets(Me me, bool inUse)
        //{
        //    foreach (var asset in me.assets)
        //        Inventories[Convert.ToUInt32(asset.appid)].MarkAsset(asset, inUse);
        //}

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
        /// Reloads all of the inventories belonging to a specified steamid64. The inventories to refresh are specified in the config file.
        /// </summary>
        /// <param name="steamId"></param>
        //public void RefreshInventories(ulong steamId)
        //{
        //    Inventories.Clear();
        //    foreach (uint appid in _config.Cfg.Inventories)
        //        Inventories.Add(appid, new Inventory(steamId, appid));
        //}

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="steamId"></param>
        /// <param name="appids"></param>
        public void RefreshInventories(uint[] appids)
        {
            Inventories.Clear();
            foreach (uint appid in appids)
                if(!Inventories.ContainsKey(appid))
                Inventories.Add(appid, new Inventory(_steamId, appid));
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
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

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
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

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
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
                //Inventory[inventory_item.classid].items
            }

            #region old
            /*
                foreach (rgDescription desc in description_list)
                    Console.WriteLine(desc.market_hash_name);
                ISteamEconomyHandler handler = new ISteamEconomyHandler();
                Dictionary<string,rgInventory_Item> data = new Dictionary<string,rgInventory_Item>();
                foreach (rgInventory_Item item in item_list)
                {
                    if (data.ContainsKey(item.classid.ToString()))
                    {
Console.WriteLine("Name | Duplicate Val | Val");
                        if (item.amount != data[item.classid.ToString()].amount)
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write("Amount: " + item.amount + " | ");
                        Console.WriteLine(data[item.classid.ToString()].amount);
                        Console.ForegroundColor = ConsoleColor.White;

                        if (item.classid != data[item.classid.ToString()].classid)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ClassID: " + item.classid + " | ");
                        Console.WriteLine(data[item.classid.ToString()].classid);
                        Console.ForegroundColor = ConsoleColor.White;

                        //may not be the same across all class id
                        if (item.id != data[item.classid.ToString()].id)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ID: " + item.id + " | ");
                        Console.WriteLine(data[item.classid.ToString()].id);
                        Console.ForegroundColor = ConsoleColor.White;

                        if (item.instanceid != data[item.classid.ToString()].instanceid)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("InstanceID: " + item.instanceid + " | ");
                        Console.WriteLine(data[item.classid.ToString()].instanceid);
                        Console.ForegroundColor = ConsoleColor.White;

                        if (item.pos != data[item.classid.ToString()].pos)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Pos: " + item.pos + " | ");
                        Console.WriteLine(data[item.classid.ToString()].pos);
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine();
                    }
                    else
                        data.Add(item.classid.ToString(), item);
                 
                }
                */
            #endregion

            //throw new ItemNotFoundException(item_name + " could not be located in " + SteamID + "'s " + app_id + " inventory.");
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

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class InventoryException : Exception
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public InventoryException() { }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="message"></param>
        public InventoryException(string message) : base(message) { }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InventoryException(string message, Exception inner) : base(message, inner){ }
    }

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

    /// <summary>
    /// An item within an inventory list.
    /// </summary>
    public class rgInventory_Item
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public ulong amount { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public ulong classid { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public ulong id { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public ulong instanceid { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public ulong pos { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        [JsonIgnore] 
        public bool inUse;

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public CEconAsset ToCEconAsset(string appId)
        {
            var asset = new CEconAsset
            {
                appid = appId,
                amount = "1",
                assetid = id.ToString(),
                contextid = "2",
                classid = classid.ToString(),
                instanceid = instanceid.ToString(),
                missing = false
            };
            return asset;
        }
    }

    /// <summary>
    /// Description object contained in the dynamic JSON returned by RequestInventory.
    /// </summary>
    public class rgDescription
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string classid { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string instanceid { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_url { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_url_large { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string icon_drag_url { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_hash_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name_color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string background_color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int tradable { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int marketable { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public int commodity { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_tradable_restriction { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string market_marketable_restriction { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Description> descriptions = new List<Description>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Action> actions = new List<Action>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public List<Tag> tags = new List<Tag>();
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public AppData app_data { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class Action
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string link { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string internal_name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string category_name { get; set; }
    }

    /// <summary>
    /// I forgot or it's obvious. TODO: Add better documentation
    /// </summary>
    public class AppData
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string def_index { get; set; }
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string quality { get; set; }
    }
}
