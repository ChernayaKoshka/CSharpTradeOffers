using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    public class Offer //ToDo: NEEDS a better name
    {
        [JsonProperty("assets")]
        public List<CEconAsset> Assets { get; set; } = new List<CEconAsset>();

        [JsonProperty("currency")]
        public List<object> Currency { get; } = new List<object>();

        [JsonProperty("ready")]
        public bool Ready { get; } = false;
    }
}