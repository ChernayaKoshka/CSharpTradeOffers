using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class GetUserGroupListBaseResult
    {
        public GetUserGroupListResult Result { get; set; }
    }
}