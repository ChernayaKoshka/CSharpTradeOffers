using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class Group
    {
        [JsonProperty("gid")]
        public ulong GroupId { get; set; } // ToDo: GroupID correct? 
    }
}