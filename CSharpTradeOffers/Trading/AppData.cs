namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    public class AppData
    {

        [JsonProperty("def_index")]
        public string DefIndex { get; set; }

        [JsonProperty("quality")]
        public string Quality { get; set; }
    }
}