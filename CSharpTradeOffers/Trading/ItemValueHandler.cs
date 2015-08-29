using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
#pragma warning disable 1591
    /// <summary>
    /// DO. NOT. USE. Under heavy construction.
    /// </summary>
    public class ItemValueHandler
    {
        private static string _path;
        private static string _apiKey;
        //private static InventoryHandler _inventoryHandler;

        public static ValuedItemsRoot ValuedItems;

        public ItemValueHandler(string path, string apiKey)
        {
            _path = path;
            _apiKey = apiKey;
            //_inventoryHandler = InventoryHandler;

            if (File.Exists(_path)) throw new Exception("Path not found.");

            File.Create(_path).Close();

            #region append

            // BuildMyString.com generated code. Please enjoy your string responsibly.

            var sb = new StringBuilder();

            sb.Append("{\r\n");
            sb.Append("  \"Items\": [\r\n");
            sb.Append("    {\r\n");
            sb.Append("      \"name\": \"scrap\",\r\n");
            sb.Append("      \"typeid\": 4,\r\n");
            sb.Append("      \"typeobj\": \"weapon\",\r\n");
            sb.Append("      \"side\":0,\r\n");
            sb.Append("      \"amount\": 2,\r\n");
            sb.Append("      \"worth\": [\r\n");
            sb.Append("        {\r\n");
            sb.Append("          \"name\": \"scrap\",\r\n");
            sb.Append("          \"typeid\": 1,\r\n");
            sb.Append("          \"typeobj\": \"scrap metal\",\r\n");
            sb.Append("          \"amount\": 1\r\n");
            sb.Append("        }\r\n");
            sb.Append("      ]\r\n");
            sb.Append("    }\r\n");
            sb.Append("  ]\r\n");
            sb.Append("}\r\n");

            #endregion

            File.WriteAllText(_path, sb.ToString());
        }

        public void RefreshValues()
        {
            ValuedItems = JsonConvert.DeserializeObject<ValuedItemsRoot>(File.ReadAllText(_path));
        }

        public TradeOffer CreateCompatibleOffer(CEconTradeOffer offer, ref InventoryHandler myInventoryHandler, ref InventoryHandler theirInventoryHandler)
        {
            TradeOffer compatibleOffer = new TradeOffer();

            foreach (CEconAsset cEconAsset in offer.items_to_give)
            {
                List<CEconAsset> assetsToAdd = FindCompatibleAssets(cEconAsset, ref myInventoryHandler, 1);

                if (assetsToAdd == null) return null;

                foreach (CEconAsset econAsset in assetsToAdd)
                    compatibleOffer.them.assets.Add(econAsset);
            }

            foreach (CEconAsset cEconAsset in offer.items_to_receive)
            {
                List<CEconAsset> assetsToAdd = FindCompatibleAssets(cEconAsset, ref theirInventoryHandler, 2);

                if (assetsToAdd == null) return null;

                foreach (CEconAsset econAsset in assetsToAdd)
                    compatibleOffer.me.assets.Add(econAsset);
            }

            return compatibleOffer;
        }

        //needs to support side
        public List<CEconAsset> FindCompatibleAssets(CEconAsset asset, ref InventoryHandler inventoryHandler, int side) //side : 0 = anyone, 1 = us, 2 = them
        {
            List<CEconAsset> compatibleAssets = null;
            foreach (ValuedItem valuedItem in ValuedItems.Items)
            {
                if (valuedItem.side != side && valuedItem.side != 0) continue; //which side should we be searching?
                bool done = false;

                switch (valuedItem.typeid)
                {
                    case 0: //exact match
                        if (asset.GetMarketHashName(_apiKey) != valuedItem.typeobj) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.worth)
                        {
                            for (int i = 0; i < valuedWorth.amount; i++)
                            {
                                rgInventory_Item rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).items.FirstOrDefault(x => !x.inUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.appid.ToString());
                                inventoryHandler.Inventories[valuedWorth.appid].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 1: //contains match
                        if (asset.GetMarketHashName(_apiKey).Contains(valuedItem.typeobj)) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.worth)
                        {
                            for (int i = 0; i < valuedWorth.amount; i++)
                            {
                                rgInventory_Item rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).items.FirstOrDefault(x => !x.inUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.appid.ToString());
                                inventoryHandler.Inventories[valuedWorth.appid].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 2: //starts with match
                        if (asset.GetMarketHashName(_apiKey).StartsWith(valuedItem.typeobj)) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.worth)
                        {
                            for (int i = 0; i < valuedWorth.amount; i++)
                            {
                                rgInventory_Item rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).items.FirstOrDefault(x => !x.inUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.appid.ToString());
                                inventoryHandler.Inventories[valuedWorth.appid].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 3: //direct class id match
                        if (asset.classid == valuedItem.typeobj)
                            foreach (ValuedWorth valuedWorth in valuedItem.worth)
                            {
                                for (int i = 0; i < valuedWorth.amount; i++)
                                {
                                    rgInventory_Item rgInventoryItem =
                                        inventoryHandler.FindUnusedItem(valuedWorth).items.FirstOrDefault(x => !x.inUse);
                                    if (rgInventoryItem == null) return null;
                                    CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.appid.ToString());
                                    inventoryHandler.Inventories[valuedWorth.appid].MarkAsset(toAdd, true);
                                    compatibleAssets.Add(toAdd);
                                }
                            }
                        break;
                    case 4: //tag category match TODO: REDO
                        var handler = new SteamEconomyHandler();
                        var IDs = new Dictionary<string, string>
                        {
                            {asset.classid, asset.instanceid}
                        };
                        AssetClassInfo assetClassInfo = handler.GetAssetClassInfo(_apiKey, Convert.ToUInt32(asset.appid),
                            IDs);

                        foreach (Tag tag in assetClassInfo.tags.Values.Where(tag => tag.category == valuedItem.typeobj)) //useless?
                        {
                            foreach (ValuedWorth valuedWorth in valuedItem.worth)
                            {
                                for (int i = 0; i < valuedWorth.amount; i++)
                                {
                                    rgInventory_Item rgInventoryItem =
                                        inventoryHandler.FindUnusedItem(valuedWorth).items.FirstOrDefault(x => !x.inUse);
                                    if (rgInventoryItem == null) return null;
                                    CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.appid.ToString());
                                    inventoryHandler.Inventories[valuedWorth.appid].MarkAsset(toAdd, true);
                                    compatibleAssets.Add(toAdd);
                                }
                            }
                            //locate item in inventory
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("TypeId was outside listed types.");
                }
            }
            return compatibleAssets;
        }

        /// <summary>
        /// TypeIds for use in trade config, mostly for reference and not actually used anywhere
        /// </summary>
        public enum TypeIds
        {
            Exact = 0,
            Contains = 1,
            StartsWith = 2,
            ClassId = 3,
            Tag = 4
        }

        [JsonObject(Title = "Worth")]
        public class ValuedWorth //TODO: definitely need a better name
        {
            public string name { get; set; }
            public uint appid { get; set; }
            public int typeid { get; set; }
            public string typeobj { get; set; }
            public int amount { get; set; }
        }

        [JsonObject(Title = "Item")]
        public class ValuedItem
        {
            public string name { get; set; }
            public int typeid { get; set; }
            public string typeobj { get; set; }
            public int side { get; set; }
            public int amount { get; set; }
            public List<ValuedWorth> worth { get; set; }
        }

        [JsonObject(Title = "RootObject")]
        public class ValuedItemsRoot //need a better name?
        {
            public List<ValuedItem> Items { get; set; }
        }
#pragma warning restore 1591
    }
}
