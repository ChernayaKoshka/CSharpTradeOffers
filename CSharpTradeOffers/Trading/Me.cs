using System;
using System.Collections.Generic;

namespace CSharpTradeOffers.Trading
{
    [Obsolete("Replaced with Offer")]
    public class Me
    {

        public List<CEconAsset> assets = new List<CEconAsset>();

        public List<object> currency = new List<object>();

        public bool ready = false;
    }
}