using System.Collections.Generic;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic officer class.
    /// </summary>
    public class Officer
    {
        /// <summary>
        /// The UInt64 version of a SteamID
        /// </summary>
        public ulong steamid { get; set; }
        /// <summary>
        /// The permission level the SteamID has.
        /// </summary>
        public int permission_level { get; set; }
        /// <summary>
        /// Turns the Officer into a KeyValuePair
        /// </summary>
        /// <returns>KeyValuePair of steamid,permissionlevel</returns>
        public KeyValuePair<ulong, int> ToKeyValuePair()
        {
            return new KeyValuePair<ulong, int>(steamid, permission_level);
        }
    }
}