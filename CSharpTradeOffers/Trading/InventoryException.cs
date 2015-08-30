using System;

namespace CSharpTradeOffers.Trading
{
    public class InventoryException : Exception
    {

        public InventoryException() { }


        /// <param name="message"></param>
        public InventoryException(string message) : base(message) { }


        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InventoryException(string message, Exception inner) : base(message, inner){ }
    }
}