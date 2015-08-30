using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    /// <summary>
    /// Description object contained in the dynamic JSON returned by RequestInventory.
    /// </summary>
    public class rgDescription
    {

        public string appid { get; set; }

        public string classid { get; set; }

        public string instanceid { get; set; }

        public string icon_url { get; set; }

        public string icon_url_large { get; set; }

        public string icon_drag_url { get; set; }

        public string name { get; set; }

        public string market_hash_name { get; set; }

        public string market_name { get; set; }

        public string name_color { get; set; }

        public string background_color { get; set; }

        public string type { get; set; }

        public int tradable { get; set; }

        public int marketable { get; set; }

        public int commodity { get; set; }

        public string market_tradable_restriction { get; set; }

        public string market_marketable_restriction { get; set; }

        public List<Description> descriptions = new List<Description>();

        public List<Action> actions = new List<Action>();

        public List<Tag> tags = new List<Tag>();

        public AppData app_data { get; set; }
    }
}