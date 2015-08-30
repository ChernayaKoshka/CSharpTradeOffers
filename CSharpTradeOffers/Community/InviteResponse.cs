using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// InviteResponse
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class InviteResponse
    {
        public string results { get; set; }
        public string groupId { get; set; }
        public bool duplicate { get; set; }
    }
}