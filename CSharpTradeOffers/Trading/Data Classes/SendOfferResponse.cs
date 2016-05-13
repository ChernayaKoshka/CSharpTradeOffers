using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class SendOfferResponse
    {
        [JsonProperty("tradeofferid")]
        public string TradeOfferId { get; set; }

        [JsonProperty("needs_email_confirmation")]
        public bool NeedsEmailConfirmation { get; set; }

        [JsonProperty("needs_mobile_confirmation")]
        public bool NeedsMobileConfirmation { get; set; }

        [JsonProperty("email_domain")]
        public string EmailDomain { get; set; }

        [JsonProperty("strError")]
        public string Error { get; set; }
    }
}