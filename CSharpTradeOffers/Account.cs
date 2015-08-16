using System.Net;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Generic account
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The bot's Sid64.
        /// </summary>
        public ulong SteamId { get; set; }
        /// <summary>
        /// The Auth Cookies for the bot.
        /// </summary>
        public CookieContainer AuthContainer = new CookieContainer();
        /// <summary>
        /// Sets the bot's auth cookie from a string of the format steamMachineAuthSID64=AuthData
        /// </summary>
        /// <param name="authstring">The auth string.</param>
        public void AddMachineAuthCookies(string authstring)
        {
            var split = authstring.Split('=');
            AuthContainer.Add(new Cookie(split[0], split[1]) { Domain = "steamcommunity.com" });
        }
    }
}
