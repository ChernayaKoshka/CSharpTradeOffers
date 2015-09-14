using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class PollResponse
    {
        public int pollid { get; set; }
        public Message[] messages { get; set; }
        public int messagelast { get; set; }
        public int timestamp { get; set; }
        public int utc_timestamp { get; set; }
        public int messagebase { get; set; }
        public int sectimeout { get; set; }
        public string error { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public int timestamp { get; set; }
        public int utc_timestamp { get; set; }
        public int accountid_from { get; set; }
        public int status_flags { get; set; }
        public int persona_state { get; set; }
        public string persona_name { get; set; }
    }
}
