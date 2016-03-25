using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class Friendslist
    {
        [JsonProperty("friends")]
        public List<Friend> Friends { get; set; }
    }
}