using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// MultiInviteResponse
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class MultiInviteResponse
    {
        public string results { get; set; }
        public string groupId { get; set; }
    }
}