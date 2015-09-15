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
        public ulong Amount { get; set; }

        [JsonProperty("classid")]
        public ulong ClassId { get; set; }

        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("instanceid")]
        public ulong InstanceId { get; set; }

        [JsonProperty("pos")]
        public ulong Pos { get; set; }

        [JsonIgnore] [JsonProperty("inUse")]
        public bool InUse;

        /// <param name="appId"></param>
        /// <returns></returns>
        public CEconAsset ToCEconAsset(uint appId)
        {
            return new CEconAsset
            {
                AppId = appId.ToString(),
                Amount = "1",
                AssetId = Id.ToString(),
                ContextId = "2",
                ClassId = ClassId.ToString(),
                InstanceId = InstanceId.ToString(),
                Missing = false
            };
        }
    }
}