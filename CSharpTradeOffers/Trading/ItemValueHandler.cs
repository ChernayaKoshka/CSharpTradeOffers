using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    internal class ItemValueHandler
    {
        private static string _path;

        public static ValuedItemsRoot ValuedItems;

        public ItemValueHandler(string path)
        {
            _path = path;

            if (File.Exists(_path)) throw new Exception("Path not found.");

            File.Create(_path).Close();

            #region append

            // BuildMyString.com generated code. Please enjoy your string responsibly.

            var sb = new StringBuilder();

            sb.Append("{\r\n");
            sb.Append("  \"Items\": [\r\n");
            sb.Append("    {\r\n");
            sb.Append("      \"name\": \"scrap\",\r\n");
            sb.Append("      \"typeid\": 0,\r\n");
            sb.Append("      \"typeobj\": \"{\\\"category\\\":\\\"Weapon\\\"}\",\r\n");
            sb.Append("      \"side\":0,\r\n");
            sb.Append("      \"amount\": 2,\r\n");
            sb.Append("      \"worth\": [\r\n");
            sb.Append("        {\r\n");
            sb.Append("          \"name\": \"scrap\",\r\n");
            sb.Append("          \"typeid\": 1,\r\n");
            sb.Append("          \"typeobj\": \"scrap metal\",\r\n");
            sb.Append("          \"amount\": 1\r\n");
            sb.Append("        }\r\n");
            sb.Append("      ]\r\n");
            sb.Append("    }\r\n");
            sb.Append("  ]\r\n");
            sb.Append("}\r\n");

            #endregion

            File.WriteAllText(_path, sb.ToString());
        }

        public void RefreshValues()
        {
            ValuedItems = JsonConvert.DeserializeObject<ValuedItemsRoot>(File.ReadAllText(_path));
        }

#pragma warning disable 1591
        [JsonObject(Title = "Worth")]
        public class ValuedWorth //TODO: definitely need a better name
        {
            public string name { get; set; }
            public int typeid { get; set; }
            public string typeobj { get; set; }
            public int amount { get; set; }
        }

        [JsonObject(Title = "Item")]
        public class ValuedItem
        {
            public string name { get; set; }
            public int typeid { get; set; }
            public string typeobj { get; set; }
            public int side { get; set; }
            public int amount { get; set; }
            public List<ValuedWorth> worth { get; set; }
        }

        [JsonObject(Title = "RootObject")]
        public class ValuedItemsRoot //need a better name?
        {
            public List<ValuedItem> Items { get; set; }
        }
#pragma warning restore 1591
    }
}
