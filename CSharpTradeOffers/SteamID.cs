using System;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Helps with conversion operations
    /// </summary>
    public class SteamIdOperations
    {
        /// <summary>
        /// Converts an AccountId to a SteamId64.
        /// </summary>
        /// <param name="accountId">AccountId to convert.</param>
        /// <returns>SteamId64 derived from AccountId</returns>
        public static ulong ConvertAccountIdToUlong(uint accountId)
        {
            return ConvertSteamIdToUlong(ConvertAccountIdToSteamId(accountId));
        }


        /// <summary>
        /// Covnerts an AccountId to a SteamId
        /// </summary>
        /// <param name="accountId">AccountId to convert to a SteamId</param>
        /// <returns>A string in the format of STEAM_0:0:0000000</returns>
        public static string ConvertAccountIdToSteamId(uint accountId)
        {
            return $"STEAM_0:{accountId & 1}:{accountId >> 1}";
        }

        /// <summary>
        /// Converts a SteamId to a SteamId64
        /// </summary>
        /// <param name="steamId">SteamId to convert to a SteamId64</param>
        /// <returns>A SteamId64.</returns>
        public static ulong ConvertSteamIdToUlong(string steamId)
        {
            string[] split = steamId.Split(':');
            return Convert.ToUInt64((Convert.ToInt32(split[2])*2) + (76561197960265728 + Convert.ToInt32(split[1])));
        }


        /// <summary>
        /// Converts a SteamId64 into a SteamId.
        /// </summary>
        /// <param name="steamId">SteamId64 to convert.</param>
        /// <returns>A string in the format of STEAM_0:0:0000000</returns>
        public static string ConvertUlongToSteamId(ulong steamId)
        {
            return $"STEAM_0:{(steamId%2)}:{((steamId - (76561197960265728 + (steamId%2)))/2)}";
        }

        /// <summary>
        /// Converts a SteamId into an AccountId
        /// </summary>
        /// <param name="steamId">SteamId to convert.</param>
        /// <returns>An AccountId</returns>
        public static uint ConvertSteamIdToAccountId(string steamId)
        {
            string[] split = steamId.Split(':');
            return Convert.ToUInt32(split[2]) << 1;
        }
    }
}
