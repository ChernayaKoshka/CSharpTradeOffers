using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// DO. NOT. USE. Under heavy construction.
    /// </summary>
    public class ItemValueHandler
    {
        private readonly string _path;

        private readonly string _apiKey;

        public static ValuedItemsRoot ValuedItems;

        public ItemValueHandler(string path, string apiKey)
        {
            _path = path;
            _apiKey = apiKey;

            if (File.Exists(_path)) throw new Exception("Path not found.");

            File.Create(_path).Close();

            #region append

            // BuildMyString.com generated code. Please enjoy your string responsibly.

            var sb = new StringBuilder();

            sb.Append("{\r\n");
            sb.Append("  \"Items\": [\r\n");
            sb.Append("    {\r\n");
            sb.Append("      \"name\": \"scrap\",\r\n");
            sb.Append("      \"appid\": 440,\r\n");
            sb.Append("      \"typeid\": 4,\r\n");
            sb.Append("      \"typeobj\": \"weapon\",\r\n");
            sb.Append("      \"side\":0,\r\n");
            sb.Append("      \"amount\": 2,\r\n");
            sb.Append("      \"worth\": [\r\n");
            sb.Append("        {\r\n");
            sb.Append("          \"name\": \"scrap\",\r\n");
            sb.Append("          \"appid\": 440,\r\n");
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
            var compatibleOffer = new TradeOffer();

            foreach (CEconAsset cEconAsset in offer.ItemsToGive)
            {
                List<CEconAsset> assetsToAdd = FindCompatibleAssets(cEconAsset, ref myInventoryHandler, TradeSide.Me);

                if (assetsToAdd == null) return null;

                foreach (CEconAsset econAsset in assetsToAdd)
                    compatibleOffer.Them.Assets.Add(econAsset);
            }

            foreach (CEconAsset cEconAsset in offer.ItemsToReceive)
            {
                List<CEconAsset> assetsToAdd = FindCompatibleAssets(cEconAsset, ref theirInventoryHandler, TradeSide.Them);

                if (assetsToAdd == null) return null;

                foreach (CEconAsset econAsset in assetsToAdd)
                    compatibleOffer.Me.Assets.Add(econAsset);
            }

            return compatibleOffer;
        }

        //needs to support side
        public List<CEconAsset> FindCompatibleAssets(CEconAsset asset, ref InventoryHandler inventoryHandler, TradeSide side) //side : 0 = anyone, 1 = us, 2 = Them
        {
            List<CEconAsset> compatibleAssets = new List<CEconAsset>();
            foreach (ValuedItem valuedItem in ValuedItems.Items)
            {
                if (valuedItem.Side != (int)side && valuedItem.Side != (int)TradeSide.None) continue; //which side should we be searching?

                switch (valuedItem.TypeId)
                {
                    case 0: //exact match
                        if (asset.GetMarketHashName(_apiKey) != valuedItem.TypeObj) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.Worth)
                        {
                            for (int i = 0; i < valuedWorth.Amount; i++)
                            {
                                RgInventoryItem rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).Items.FirstOrDefault(x => !x.InUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.AppId.ToString());
                                inventoryHandler.Inventories[valuedWorth.AppId].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 1: //contains match
                        if (asset.GetMarketHashName(_apiKey).Contains(valuedItem.TypeObj)) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.Worth)
                        {
                            for (int i = 0; i < valuedWorth.Amount; i++)
                            {
                                RgInventoryItem rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).Items.FirstOrDefault(x => !x.InUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.AppId.ToString());
                                inventoryHandler.Inventories[valuedWorth.AppId].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 2: //starts with match
                        if (asset.GetMarketHashName(_apiKey).StartsWith(valuedItem.TypeObj)) break;
                        foreach (ValuedWorth valuedWorth in valuedItem.Worth)
                        {
                            for (int i = 0; i < valuedWorth.Amount; i++)
                            {
                                RgInventoryItem rgInventoryItem =
                                    inventoryHandler.FindUnusedItem(valuedWorth).Items.FirstOrDefault(x => !x.InUse);
                                if (rgInventoryItem == null) return null;
                                CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.AppId.ToString());
                                inventoryHandler.Inventories[valuedWorth.AppId].MarkAsset(toAdd, true);
                                compatibleAssets.Add(toAdd);
                            }
                        }
                        break;
                    case 3: //direct class id match
                        if (asset.ClassId == valuedItem.TypeObj)
                            foreach (ValuedWorth valuedWorth in valuedItem.Worth)
                            {
                                for (int i = 0; i < valuedWorth.Amount; i++)
                                {
                                    RgInventoryItem rgInventoryItem =
                                        inventoryHandler.FindUnusedItem(valuedWorth).Items.FirstOrDefault(x => !x.InUse);
                                    if (rgInventoryItem == null) return null;
                                    CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.AppId.ToString());
                                    inventoryHandler.Inventories[valuedWorth.AppId].MarkAsset(toAdd, true);
                                    compatibleAssets.Add(toAdd);
                                }
                            }
                        break;
                    case 4: //tag category match TODO: REDO
                        var handler = new SteamEconomyHandler(_apiKey);
                        var ids = new Dictionary<string, string>
                        {
                            {asset.ClassId, asset.InstanceId}
                        };
                        AssetClassInfo assetClassInfo = handler.GetAssetClassInfo(Convert.ToUInt32(asset.AppId),
                            ids);

                        foreach (Tag tag in assetClassInfo.Tags.Values.Where(tag => tag.Category == valuedItem.TypeObj)) //useless?
                        {
                            foreach (ValuedWorth valuedWorth in valuedItem.Worth)
                            {
                                for (int i = 0; i < valuedWorth.Amount; i++)
                                {
                                    RgInventoryItem rgInventoryItem =
                                        inventoryHandler.FindUnusedItem(valuedWorth).Items.FirstOrDefault(x => !x.InUse);
                                    if (rgInventoryItem == null) return null;
                                    CEconAsset toAdd = rgInventoryItem.ToCEconAsset(valuedWorth.AppId.ToString());
                                    inventoryHandler.Inventories[valuedWorth.AppId].MarkAsset(toAdd, true);
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
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("appid")]
            public uint AppId { get; set; }
            [JsonProperty("typeid")]
            public int TypeId { get; set; }
            [JsonProperty("typeobj")]
            public string TypeObj { get; set; }
            [JsonProperty("amount")]
            public int Amount { get; set; }
        }

        [JsonObject(Title = "Item")]
        public class ValuedItem
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("typeid")]
            public int TypeId { get; set; }
            [JsonProperty("typeobj")]
            public string TypeObj { get; set; }
            [JsonProperty("side")]
            public int Side { get; set; }
            [JsonProperty("amount")]
            public int Amount { get; set; }
            [JsonProperty("worth")]
            public List<ValuedWorth> Worth { get; set; }
        }

        [JsonObject(Title = "RootObject")]
        public class ValuedItemsRoot //need a better name?
        {
            [JsonProperty("Items")] // ToDo: Items in camelcase? Was Uppercase before 
            public List<ValuedItem> Items { get; set; }
        }
    }
}
