using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "TradeOffersReceieved")]
    public class CEconTradeOffer
    {

        public string tradeofferid { get; set; }

        public uint accountid_other { get; set; }

        public string message { get; set; }

        public int expiration_time { get; set; }

        public int trade_offer_state { get; set; }

        public List<CEconAsset> items_to_give { get; set; }

        public List<CEconAsset> items_to_receive { get; set; }

        public bool is_our_offer { get; set; }

        public ulong time_created { get; set; }

        public ulong time_updated { get; set; }

        public string tradeid { get; set; }

        public bool from_real_time_trade { get; set; }
    }
}