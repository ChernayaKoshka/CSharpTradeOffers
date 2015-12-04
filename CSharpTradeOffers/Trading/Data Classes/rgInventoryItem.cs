using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// An item within an inventory list.
    /// </summary>
    [JsonObject(Title = "rgInventory_Item")]
    public class RgInventoryItem
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("classid")]
        public long ClassId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("instanceid")]
        public long InstanceId { get; set; }

        [JsonProperty("pos")]
        public long Pos { get; set; }

        [JsonIgnore]
        [JsonProperty("inUse")]
        public bool InUse { get; set; }

        /// <param name="appId"></param>
        /// <returns></returns>
        public CEconAsset ToCEconAsset(uint appId)
        {
            return new CEconAsset
            {
                AppId = appId,
                Amount = 1,
                AssetId = Id,
                ContextId = 2,
                ClassId = ClassId,
                InstanceId = InstanceId,
                Missing = false
            };
        }
    }
}