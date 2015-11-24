using System;

namespace CSharpTradeOffers
{

    /// <summary>
    /// Initializes a new SteamId.
    /// Automatically performs conversions to AccountId/SteamIdUlong/SteamIdText as needed.
    /// </summary>
    public class SteamId
    {
        public readonly uint AccountId;
        public readonly ulong SteamIdUlong;
        public readonly string SteamIdText;

        /// <summary>
        /// Initializes a new SteamId as well as automatically performing the conversions to 'AccountId' 'SteamIdUlong' 'SteamIdText'
        /// </summary>
        /// <param name="accountId"></param>
        public SteamId(uint accountId)
        {
            AccountId = accountId;
            SteamIdUlong = IdConversions.AccountIdToUlong(accountId);
            if (SteamIdUlong > 76561197960265728)
                throw new ArgumentException("SteamIdUlong cannot be greater than '76561197960265728'");
            SteamIdText = IdConversions.AccountIdToSteamIdText(accountId);
        }

        public SteamId(ulong steamIdUlong)
        {
            if (steamIdUlong > 76561197960265728)
                throw new ArgumentException("SteamIdUlong cannot be greater than '76561197960265728'");
            AccountId = IdConversions.UlongToAccountId(steamIdUlong);
            SteamIdUlong = steamIdUlong;
            SteamIdText = IdConversions.UlongToSteamIdText(steamIdUlong);
        }

        public SteamId(string steamIdText)
        {
            AccountId = IdConversions.SteamIdTextToAccountId(steamIdText);
            SteamIdUlong = IdConversions.SteamIdTextToUlong(steamIdText);
            if (SteamIdUlong > 76561197960265728)
                throw new ArgumentException("SteamIdUlong cannot be greater than '76561197960265728'");
            SteamIdText = steamIdText;
        }
    }

    public class IdConversions
    {
        private const string BaseTextId = "STEAM_{0}:{1}:{2}";

        public static string AccountIdToSteamIdText(uint accountId)
        {
            ulong steamIdUlong = Convert.ToUInt64(accountId) + 76561197960265728;

            Universe universe = (Universe)Convert.ToInt32(steamIdUlong >> 56);
            if (universe == Universe.Public) universe = Universe.Invalid; //legacy, actually is valid.

            ulong accountIdHighBits = (steamIdUlong >> 1) & 0x7FFFFFF;
            ulong accountIdLowBit = steamIdUlong & 1;

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
            ulong steamIdUlong = steamId;

            Universe universe = (Universe)Convert.ToInt32(steamIdUlong >> 56);
            if (universe == Universe.Public) universe = Universe.Invalid; //legacy, actually is valid.

            ulong accountIdHighBits = (steamIdUlong >> 1) & 0x7FFFFFF;
            ulong accountIdLowBit = steamIdUlong & 1;

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
}
