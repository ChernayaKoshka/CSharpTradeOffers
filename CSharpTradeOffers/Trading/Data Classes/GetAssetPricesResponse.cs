using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    [JsonObject(Title = "RootObject")]
    public class GetAssetPricesBaseResponse
    {
        [JsonProperty("result")]
        public GetAssetPricesResponse Result { get; set; }
    }
    
    public class GetAssetPricesResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }
    }

    public class Asset
    {
        [JsonProperty("prices")]
        public Dictionary<string, string> Prices { get; set; }
        [JsonProperty("original_prices")]
        public Dictionary<string, string> OriginalPrices { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("class")]
        public Class[] Class { get; set; }
        [JsonProperty("classid")]
        public string ClassId { get; set; }
    }

    [JsonObject(Title = "class")]
    public class Class
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
