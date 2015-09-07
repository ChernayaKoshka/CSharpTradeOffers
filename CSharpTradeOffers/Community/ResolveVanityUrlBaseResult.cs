using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ResolveVanityUrlBaseResult
    {
        [JsonProperty("response")]
        public ResolveVanityUrlResult Response { get; set; }
    }
}