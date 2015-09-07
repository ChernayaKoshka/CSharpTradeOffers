using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// InviteResponse
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class InviteResponse
    {
        [JsonProperty("results")]
        public string Results { get; set; }
        [JsonProperty("groupId")]
        public string GroupId { get; set; }
        [JsonProperty("duplicate")]
        public bool Duplicate { get; set; }
    }
}