using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class PollResponse
    {
        [JsonProperty("pollid")]
        public int PollId { get; set; }
        [JsonProperty("messages")]
        public Message[] Messages { get; set; }
        [JsonProperty("messagelast")]
        public int MessageLast { get; set; }
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonProperty("utc_timestamp")]
        public int UtcTimestamp { get; set; }
        [JsonProperty("messagebase")]
        public int MessageBase { get; set; }
        [JsonProperty("sectimeout")]
        public int SecTimeout { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class Message
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonProperty("utc_timestamp")]
        public int UtcTimestamp { get; set; }
        [JsonProperty("accountid_from")]
        public int AccountIdFrom { get; set; }
        [JsonProperty("status_flags")]
        public int StatusFlags { get; set; }
        [JsonProperty("persona_state")]
        public int PersonaState { get; set; }
        [JsonProperty("persona_name")]
        public string PersonaName { get; set; }
    }
}
