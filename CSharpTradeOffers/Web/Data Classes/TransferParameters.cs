using Newtonsoft.Json;

namespace CSharpTradeOffers.Web
{
    [JsonObject(Title = "Transfer_Parameters")]
    public class TransferParameters
    {
        [JsonProperty("steamid")]
        public string Steamid { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("auth")]
        public string Auth { get; set; }
        [JsonProperty("remember_login")]
        public bool RememberLogin { get; set; }
        [JsonProperty("token_secure")]
        public string TokenSecure { get; set; }
    }
}
