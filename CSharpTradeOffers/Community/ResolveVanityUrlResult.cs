namespace CSharpTradeOffers.Community
{
    using Newtonsoft.Json;

    public class ResolveVanityUrlResult
    {
        [JsonProperty("steamid")]
        public string SteamId { get; set; }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}