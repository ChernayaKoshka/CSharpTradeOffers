using System.Net;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Generic account
    /// </summary>
    public class Account
    {
        public Account(ulong steamId)
        {
            //add validation later
            SteamId = steamId;
        }

        /// <summary>
        /// The bot's Sid64.
        /// </summary>
        public ulong SteamId { get; }

        /// <summary>
        /// The Auth Cookies for the bot.
        /// </summary>
        public CookieContainer AuthContainer = new CookieContainer();
    }
}
