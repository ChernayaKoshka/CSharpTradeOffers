using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class Action
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}