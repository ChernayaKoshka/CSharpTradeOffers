using System;
using System.Net.NetworkInformation;

namespace CSharpTradeOffers
{

    /// <summary>
    /// Initializes a new SteamId.
    /// Automatically performs conversions to AccountId/SteamId64/SteamIdText as needed.
    /// </summary>
    public class SteamId
    {
        public readonly uint AccountId;
        public readonly ulong SteamId64;
        public readonly string SteamIdText;

        public SteamId(uint accountId)
        {
            AccountId = accountId;
            SteamId64 = IdConversions.AccountIdToUlong(accountId);
            SteamIdText = IdConversions.AccountIdToSteamIdText(accountId);
        }

        public SteamId(ulong steamId64)
        {
            AccountId = IdConversions.UlongToAccountId(steamId64);
            SteamId64 = steamId64;
            SteamIdText = IdConversions.UlongToSteamIdText(steamId64);
        }

        public SteamId(string steamIdText)
        {
            AccountId = IdConversions.SteamIdTextToAccountId(steamIdText);
            SteamId64 = IdConversions.SteamIdTextToUlong(steamIdText);
            SteamIdText = steamIdText;
        }
    }

    public class IdConversions
    {
        private const string BaseTextId = "STEAM_{0}:{1}:{2}";

        public static string AccountIdToSteamIdText(uint accountId)
        {
            ulong steamId64 = Convert.ToUInt64(accountId) + 76561197960265728;

            Universe universe = (Universe)Convert.ToInt32(steamId64 >> 56);
            if (universe == Universe.Public) universe = Universe.Invalid; //legacy, actually is valid.

            ulong accountIdHighBits = (steamId64 >> 1) & 0x7FFFFFF;
            ulong accountIdLowBit = steamId64 & 1;

            return string.Format(BaseTextId, (int)universe, accountIdLowBit, accountIdHighBits);
        }

        public static ulong AccountIdToUlong(uint accountId)
        {
            return Convert.ToUInt64(accountId) + 76561197960265728;
        }

        public static uint SteamIdTextToAccountId(string steamIdText)
        {
            string[] split = steamIdText.Split(':'); //[0] = Steam_X
                                                     //[1] = low bits
                                                     //[2] = high 
            return Convert.ToUInt32((Convert.ToInt32(split[2]) * 2) + (Convert.ToInt32(split[1])));
        }

        public static ulong SteamIdTextToUlong(string steamIdText)
        {
            string[] split = steamIdText.Split(':');
            return Convert.ToUInt64((Convert.ToInt32(split[2]) * 2) + (76561197960265728 + Convert.ToInt32(split[1])));
        }

        public static uint UlongToAccountId(ulong steamId)
        {
            return Convert.ToUInt32(steamId - 76561197960265728);
        }

        public static string UlongToSteamIdText(ulong steamId)
        {
            ulong steamId64 = steamId;

            Universe universe = (Universe)Convert.ToInt32(steamId64 >> 56);
            if (universe == Universe.Public) universe = Universe.Invalid; //legacy, actually is valid.

            ulong accountIdHighBits = (steamId64 >> 1) & 0x7FFFFFF;
            ulong accountIdLowBit = steamId64 & 1;

            return string.Format(BaseTextId, (int)universe, accountIdLowBit, accountIdHighBits);
        }
    }

    public enum Universe
    {
        Invalid = 0,
        Public = 1,
        Beta = 2,
        Interal = 3,
        Dev = 4
    }

    /// <summary>
    /// Helps with conversion operations
    /// </summary>
    [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
    public class SteamIdOperations
    {
        /// <summary>
        /// Converts an AccountId to a SteamId64.
        /// </summary>
        /// <param name="accountId">AccountId to convert.</param>
        /// <returns>SteamId64 derived from AccountId</returns>
        [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
        public static ulong ConvertAccountIdToUlong(uint accountId)
        {
            return ConvertSteamIdToUlong(ConvertAccountIdToSteamId(accountId));
        }


        /// <summary>
        /// Covnerts an AccountId to a SteamId
        /// </summary>
        /// <param name="accountId">AccountId to convert to a SteamId</param>
        /// <returns>A string in the format of STEAM_0:0:0000000</returns>
        [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
        public static string ConvertAccountIdToSteamId(uint accountId)
        {
            return $"STEAM_0:{accountId & 1}:{accountId >> 1}";
        }

        /// <summary>
        /// Converts a SteamId to a SteamId64
        /// </summary>
        /// <param name="steamId">SteamId to convert to a SteamId64</param>
        /// <returns>A SteamId64.</returns>
        [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
        public static ulong ConvertSteamIdToUlong(string steamId)
        {
            string[] split = steamId.Split(':');
            return Convert.ToUInt64((Convert.ToInt32(split[2]) * 2) + (76561197960265728 + Convert.ToInt32(split[1])));
        }


        /// <summary>
        /// Converts a SteamId64 into a SteamId.
        /// </summary>
        /// <param name="steamId">SteamId64 to convert.</param>
        /// <returns>A string in the format of STEAM_0:0:0000000</returns>
        [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
        public static string ConvertUlongToSteamId(ulong steamId)
        {
            return $"STEAM_0:{(steamId % 2)}:{((steamId - (76561197960265728 + (steamId % 2))) / 2)}";
        }

        /// <summary>
        /// Converts a SteamId into an AccountId
        /// </summary>
        /// <param name="steamId">SteamId to convert.</param>
        /// <returns>An AccountId</returns>
        [Obsolete("Please used IdConversions instead. Better yet, use SteamId instead.")]
        public static uint ConvertSteamIdToAccountId(string steamId)
        {
            string[] split = steamId.Split(':');
            return Convert.ToUInt32(split[2]) << 1;
        }
    }
}
