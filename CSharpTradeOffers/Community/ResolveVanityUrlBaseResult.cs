using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class ResolveVanityUrlBaseResult
    {

        public ResolveVanityUrlResult response { get; set; }
    }
}