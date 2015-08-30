using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class SendOfferResponse
    {


        public string tradeofferid { get; set; }

        public bool needs_email_confirmation { get; set; }

        public string email_domain { get; set; }
    }
}