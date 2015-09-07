namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    public class Action
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}