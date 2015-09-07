namespace CSharpTradeOffers.Community
{
    using Newtonsoft.Json;

    public class Group
    {
        [JsonProperty("gid")]
        public ulong GroupId { get; set; } // ToDo: GroupID correct? 
    }
}