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
        public readonly CookieContainer AuthContainer = new CookieContainer();

        /// <summary>
        /// Adds the steamMachineAuth cookie to the AuthContainer.
        /// This can also be used for any string separated by [name]=[value]
        /// </summary>
        /// <param name="authstring">String to turn into a cookie and add.</param>
        public void AddMachineAuthCookies(string authstring)
        {
            string[] split = authstring.Split('=');
            AuthContainer.Add(new Cookie(split[0], split[1]) { Domain = "steamcommunity.com" });
        }
    }
}
