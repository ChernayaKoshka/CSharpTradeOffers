using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "Response")]
    public class TradeOffersList
    {
        [JsonProperty("trade_offers_received")]
        public List<CEconTradeOffer> TradeOffersReceived { get; set; }
        
        [JsonProperty("trade_offers_sent")]
        public List<CEconTradeOffer> TradeOffersSent { get; set; }
        
        [JsonProperty("descriptions")]
        public List<RgDescription> Descriptions { get; set; }
    }
}
