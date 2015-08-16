namespace CSharpTradeOffers
{
    /// <summary>
    /// Various extension methods used in the library.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the bool variable to its integer representation of 0 for false and 1 for true.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int IntValue(this bool b)
        {
            return b ? 1 : 0;
        }
    }
}
