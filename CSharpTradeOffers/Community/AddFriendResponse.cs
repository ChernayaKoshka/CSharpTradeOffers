using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class AddFriendResponse
    {
        [JsonProperty("invited")]
        public string[] Invited { get; set; }
        [JsonProperty("success")]
        public int Success { get; set; }
    }

}
