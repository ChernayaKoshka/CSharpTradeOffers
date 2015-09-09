namespace CSharpTradeOffers.Trading
{
    using Newtonsoft.Json;

    public class Tag
    {
        [JsonProperty("internal_name")]
        public string InternalName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
    }
}