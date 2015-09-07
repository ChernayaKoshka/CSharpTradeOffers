using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// MultiInviteResponse
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class MultiInviteResponse
    {
        [JsonProperty("results")]
        public string Results { get; set; }
        [JsonProperty("groupId")]
        public string GroupId { get; set; }
    }
}