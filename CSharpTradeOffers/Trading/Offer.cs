using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    public class Offer //ToDo: NEEDS a better name
    {
        [JsonProperty("assets")]
        public List<CEconAsset> Assets = new List<CEconAsset>();

        [JsonProperty("currency")]
        public List<object> Currency = new List<object>();

        [JsonProperty("ready")]
        public bool Ready = false;
    }
}