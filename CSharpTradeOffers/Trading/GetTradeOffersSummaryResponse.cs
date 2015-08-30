using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "Response")]
    public class GetTradeOffersSummaryResponse
    {

        public int pending_received_count { get; set; }

        public int new_received_count { get; set; }

        public int updated_received_count { get; set; }

        public int historical_received_count { get; set; }

        public int pending_sent_count { get; set; }

        public int newly_accepted_sent_count { get; set; }

        public int updated_sent_count { get; set; }

        public int historical_sent_count { get; set; }
    }
}