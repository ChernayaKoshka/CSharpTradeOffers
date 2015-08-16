using System;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Helps with conversion operations
    /// </summary>
    public class SteamIdOperations
    {
        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
        public static ulong ConvertAccountIdtoUInt64(uint accountid)
        {
            return ConvertSteamIdtoULong(ConvertAccountIdToSteamId(accountid));
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
        public static string ConvertAccountIdToSteamId(uint accountid)
        {
            return $"STEAM_0:{accountid & 1}:{accountid >> 1}";
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static ulong ConvertSteamIdtoULong(string sid)
        {
            string[] split = sid.Split(':');
            return Convert.ToUInt64((Convert.ToInt32(split[2])*2) + (76561197960265728 + Convert.ToInt32(split[1])));
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="steamid"></param>
        /// <returns></returns>
        public static string ConvertUlongToSteamId(ulong steamid)
        {
            return $"Steam_0:{(steamid%2)}:{((steamid - (76561197960265728 + (steamid%2)))/2)}";
        }

        /// <summary>
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        /// <param name="steamid"></param>
        /// <returns></returns>
        public static uint ConvertSteamIdToAccountId(string steamid)
        {
            string[] split = steamid.Split(':');
            return Convert.ToUInt32(split[2]) << 1;
        }
    }
}
