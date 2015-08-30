using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class TradeOffer
    {

        public bool newversion = true;

        public int version = 2;

        public Offer me = new Offer();

        public Offer them = new Offer();
    }
}