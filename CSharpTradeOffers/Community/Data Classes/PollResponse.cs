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
        public long Timestamp { get; set; }

        [JsonProperty("utc_timestamp")]
        public long UtcTimestamp { get; set; }

        [JsonProperty("accountid_from")]
        public uint AccountIdFrom { get; set; }

        [JsonProperty("status_flags")]
        public int StatusFlags { get; set; }

        [JsonProperty("persona_state")]
        public int PersonaState { get; set; }

        [JsonProperty("persona_name")]
        public string PersonaName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    /*
    {
	"pollid": 9,
	"messages": [
		{
			"type": "saytext",
			"timestamp": 96157804,
			"utc_timestamp": 1450136250,
			"accountid_from": 100049908,
			"text": "test2"
		}
	]
	,
	"messagelast": 43,
	"timestamp": 96157804,
	"utc_timestamp": 1450136250,
	"messagebase": 42,
	"sectimeout": 1,
	"error": "OK"
    }
    */
}
