using System.Collections.Generic;

namespace CSharpTradeOffers.Community
{
    using Newtonsoft.Json;

    public class Friendslist
    {
        [JsonProperty("friends")]
        public List<Friend> Friends { get; set; }
    }
}