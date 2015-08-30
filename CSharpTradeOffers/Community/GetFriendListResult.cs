using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetFriendListResult
    {
        public Friendslist friendslist { get; set; }
    }
}