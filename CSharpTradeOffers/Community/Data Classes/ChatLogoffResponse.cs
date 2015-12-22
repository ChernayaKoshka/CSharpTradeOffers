using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ChatLogoffResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
