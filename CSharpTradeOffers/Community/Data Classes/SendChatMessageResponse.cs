using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class SendChatMessageResponse
    {
        public int utc_timestamp { get; set; }
        public string error { get; set; }
    }

}
