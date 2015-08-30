using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    public class Offer //NEEDS a better name
    {
        public List<CEconAsset> assets = new List<CEconAsset>();

        public List<object> currency = new List<object>();

        public bool ready = false;
    }
}