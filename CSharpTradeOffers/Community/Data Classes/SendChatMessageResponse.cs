using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class SendChatMessageResponse
    {
        [JsonProperty("utc_timestamp")]
        public int UtcTimestamp { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

}
