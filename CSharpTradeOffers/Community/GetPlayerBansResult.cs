using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetPlayerBansResult
    {
        [JsonProperty("playersbans")]
        public List<PlayerBans> PlayerBans { get; set; } //better name?
    }
}