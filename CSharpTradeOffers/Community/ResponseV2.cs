using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Response")]
    public class ResponseV2
    {
        public List<PlayerSummary> PlayersSummaries { get; set; } 
    }
}