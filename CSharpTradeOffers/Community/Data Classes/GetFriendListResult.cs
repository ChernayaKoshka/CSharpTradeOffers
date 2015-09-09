using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetFriendListResult
    {
        [JsonProperty("friendslist")]
        public Friendslist Friendslist { get; set; }
    }
}