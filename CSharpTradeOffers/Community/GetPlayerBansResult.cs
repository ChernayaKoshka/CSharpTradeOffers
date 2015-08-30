using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetPlayerBansResult
    {
        public List<PlayerBans> playersbans { get; set; } //better name?
    }
}