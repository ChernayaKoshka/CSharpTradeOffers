using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "Response")]
    public class GetUserGroupListResult
    {
        public bool success { get; set; }

        public List<Group> groups { get; set; }
    }
}