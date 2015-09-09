using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// A generic RootConfig object containing configuration information.
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class RootConfig
    {
        /// <summary>
        /// Username to automatically log in to.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password to use to automatically log in.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Special bytes sent to Steam to prove the user is the account holder.
        /// </summary>"
        public string SteamMachineAuth { get; set; }
        /// <summary>
        /// A special key retrieved from https://steamcommunity.com/dev/apikey
        /// The key MUST be from the bot's account.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Will be said upon a successful friend add.
        /// </summary>
        public string GreetMessage { get; set; }
        /// <summary>
        /// Determines whether or not the bot should attempt to friend all Officers.
        /// </summary>
        public bool AddOfficers { get; set; }
        /// <summary>
        /// Changes whether or not the Debug.Log should output verbose information.
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// Log all chat into .txt files with the date and time. A new file is generated every five minutes.
        /// </summary>
        public bool LogChat { get; set; }
        /// <summary>
        /// I can't remember.
        /// </summary>
        public bool Log { get; set; }
        /// <summary>
        /// List of Officer objects.
        /// </summary>
        public List<Officer> Officers { get; set; }
        /// <summary>
        /// List of Command_Permission objects.
        /// </summary>
        public List<Command_Permission> CommandPermissions { get; set; }
        /// <summary>
        /// Inventories to create on load, refreshInventories method from Inventory handler also uses these.
        /// </summary>
        public List<uint> Inventories { get; set; }
        /// <summary>
        /// Define users that are not allowed to use this bot.
        /// </summary>
        public List<ulong> BannedUsers { get; set; }
    }
}