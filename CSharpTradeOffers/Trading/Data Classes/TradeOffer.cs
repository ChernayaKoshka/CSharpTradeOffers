using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeOffer
    {
        [JsonProperty("newversion")]
        public bool NewVersion { get; } = true;

        [JsonProperty("version")]
        public int Version { get; } = 2;

        [JsonProperty("me")]
        public Offer Me { get; } = new Offer();

        [JsonProperty("them")]
        public Offer Them { get; } = new Offer();
    }
}