using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class ChatLogMessage
    {
        [JsonProperty("m_unAccountID")]
        public int AccountID { get; set; }

        [JsonProperty("m_tsTimestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("m_strMessage")]
        public string Message { get; set; }
    }

}
