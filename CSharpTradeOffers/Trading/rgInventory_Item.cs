using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// An item within an inventory list.
    /// </summary>
    public class rgInventory_Item
    {

        public ulong amount { get; set; }

        public ulong classid { get; set; }

        public ulong id { get; set; }

        public ulong instanceid { get; set; }

        public ulong pos { get; set; }

        [JsonIgnore] 
        public bool inUse;


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
}