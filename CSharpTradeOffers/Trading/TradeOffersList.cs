using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "Response")]
    public class TradeOffersList
    {
        [JsonProperty("trade_offers_received")]
        public List<CEconTradeOffer> TradeOffersReceived { get; set; }
    }
}