using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic trade config, in progress suggested to not use
    /// </summary>
    [Obsolete("Replaced by ItemValueHandler")]
    public static class TradeConfig
    {
        /// <summary>
        /// The meat of the config
        /// </summary>
        public static Trades TradesConfig = new Trades();

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public class ConfigAsset
        {
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public uint AppId { get; set; }
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public int TypeId { get; set; }
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public string TypeObj { get; set; }
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public int Amount { get; set; }
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public class AcceptableTrade
        {
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public List<ConfigAsset> Me { get; set; }
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
            public List<ConfigAsset> Them { get; set; }
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        [JsonObject(Title = "RootObject")]
        public class Trades
        {
            /// <summary>
            /// I forgot or it's obvious. TODO: Add better documentation
            /// </summary>
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

        /// <summary>
        /// Writes the changes made to the trade config.
        /// </summary>
        /// <param name="towrite"></param>
        public static void WriteChanges(Trades towrite)
        {
            File.WriteAllText("trades.cfg", JsonConvert.SerializeObject(towrite));
        }
    }
}