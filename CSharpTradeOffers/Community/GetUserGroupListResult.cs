using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Response")]
    public class GetUserGroupListResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
    }
}