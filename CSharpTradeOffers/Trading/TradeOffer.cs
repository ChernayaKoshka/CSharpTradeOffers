using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeOffer
    {
        [JsonProperty("newversion")]
        public bool NewVersion = true;

        [JsonProperty("version")]
        public int Version = 2;

        [JsonProperty("me")]
        public Offer Me = new Offer();

        [JsonProperty("them")]
        public Offer Them = new Offer();
    }
}