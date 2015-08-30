using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "Response")]
    public class TradeOffersList
    {

        public List<CEconTradeOffer> trade_offers_received { get; set; }
    }
}