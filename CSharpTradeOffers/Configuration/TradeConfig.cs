using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    public static class TradeConfig
    {
        public static Trades TradesConfig = new Trades();

        public class ConfigAsset
        {
            public uint AppId { get; set; }
            public int TypeId { get; set; }
            public string TypeObj { get; set; }
            public int Amount { get; set; }
        }

        public class AcceptableTrade
        {
            public List<ConfigAsset> Me { get; set; }
            public List<ConfigAsset> Them { get; set; }
        }

        [JsonObject(Title = "RootObject")]
        public class Trades
        {
            public List<AcceptableTrade> AcceptableTrades { get; set; }
        }

        /// <summary>
        /// Reloads the configuration file (trades.cfg). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A Trades object.</returns>
        public static void Reload()
        {
            if (!File.Exists("trades.cfg"))
            {                 
                File.Create("trades.cfg").Close();

                #region append
                // BuildMyString.com generated code. Please enjoy your string responsibly.

                StringBuilder sb = new StringBuilder();

                sb.Append("{\r\n");
                sb.Append("  \"AcceptableTrades\": [\r\n");
                sb.Append("    {\r\n");
                sb.Append("      \"Me\": [\r\n");
                sb.Append("        {\r\n");
                sb.Append("          \"AppId\": 440,\r\n");
                sb.Append("          \"TypeId\": 1,\r\n");
                sb.Append("          \"TypeObj\": \"box\",\r\n");
                sb.Append("          \"Amount\": 1\r\n");
                sb.Append("        }\r\n");
                sb.Append("      ],\r\n");
                sb.Append("      \"Them\": [\r\n");
                sb.Append("        {\r\n");
                sb.Append("          \"AppId\": 440,\r\n");
                sb.Append("          \"TypeId\": 1,\r\n");
                sb.Append("          \"TypeObj\": \"box\",\r\n");
                sb.Append("          \"Amount\": 1\r\n");
                sb.Append("        }\r\n");
                sb.Append("      ]\r\n");
                sb.Append("    }\r\n");
                sb.Append("  ]\r\n");
                sb.Append("}\r\n");
                #endregion

                File.WriteAllText("trades.cfg", sb.ToString());
            }
            TradesConfig = JsonConvert.DeserializeObject<Trades>(File.ReadAllText("trades.cfg"));
        }

        public static void WriteChanges(Trades towrite)
        {
            File.WriteAllText("trades.cfg", JsonConvert.SerializeObject(towrite));
        }
    }
}