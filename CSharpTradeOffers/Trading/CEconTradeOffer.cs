using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "TradeOffersReceieved")]
    public class CEconTradeOffer
    {

        [JsonProperty("tradeofferid")]
        public string TradeofferId { get; set; }

        [JsonProperty("accountid_other")]
        public uint AccountIdOther { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("expiration_time")]
        public int ExpirationTime { get; set; }

        [JsonProperty("trade_offer_state")]
        public int TradeOfferState { get; set; }

        [JsonProperty("items_to_give")]
        public List<CEconAsset> ItemsToGive { get; set; }

        [JsonProperty("items_to_receive")]
        public List<CEconAsset> ItemsToReceive { get; set; }

        [JsonProperty("is_our_offer")]
        public bool IsOurOffer { get; set; }

        [JsonProperty("time_created")]
        public ulong TimeCreated { get; set; }

        [JsonProperty("time_updated")]
        public ulong TimeUpdated { get; set; }

        [JsonProperty("tradeid")]
        public string TradeId { get; set; }

        [JsonProperty("from_real_time_trade")]
        public bool FromRealTimeTrade { get; set; }
    }
}