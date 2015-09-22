using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class WebPresenceOAuthLogonResponse
    {
        [JsonProperty("steamid")]
        public string SteamId { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("umqid")]
        public string UmqId { get; set; }
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonProperty("utc_timestamp")]
        public int UtcTimestamp { get; set; }
        [JsonProperty("message")]
        public int Message { get; set; }
        [JsonProperty("push")]
        public int Push { get; set; }
    }
}
