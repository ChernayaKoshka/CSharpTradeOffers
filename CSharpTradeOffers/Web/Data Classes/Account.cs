using System;
using System.Net;

namespace CSharpTradeOffers.Web
{
    /// <summary>
    /// Generic account
    /// </summary>
    public class Account
    {
        public Account(ulong steamId)
        {
            //add validation later
            if (steamId < 76561197960265728)
                throw new ArgumentException("Invalid SteamId64, must be greater than 76561197960265728!");
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

        private string _steamMachineAuth;

        /// <summary>
        /// Sets the SteamMachineAuth string as well as adding the auth string to the AuthContainer.
        /// </summary>
        public string SteamMachineAuth
        {
            get { return _steamMachineAuth; }
            set { _steamMachineAuth = value; AddMachineAuthCookies(value); }
        }

        /// <summary>
        /// Adds the steamMachineAuth cookie to the AuthContainer.
        /// This can also be used for any string separated by [name]=[value]
        /// </summary>
        /// <param name="authstring">String to turn into a cookie and add.</param>
        private void AddMachineAuthCookies(string authstring)
        {
            string[] split = authstring.Split('=');
            AuthContainer.Add(new Cookie(split[0], split[1]) { Domain = "steamcommunity.com" });
        }
    }
}
