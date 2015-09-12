using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Json
{
    public class RsaKey
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("publickey_mod")]
        public string PublicKeyMod { get; set; }
        [JsonProperty("publickey_exp")]
        public string PublicKeyExp { get; set; }
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
    }

}
