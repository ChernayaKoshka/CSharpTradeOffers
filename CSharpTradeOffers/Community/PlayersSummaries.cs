using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Players")]
    public class PlayersSummaries
    {
        public List<PlayerSummary> PlayerSummaries { get; set; }
    }
}