using Newtonsoft.Json;

namespace CSharpTradeOffers.Web
{
    public class RsaKey
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("publickey_mod")]
        public string PublicKeyMod { get; set; }
        [JsonProperty("publickey_exp")]
        public string PublicKeyExp { get; set; }
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
    }

}
