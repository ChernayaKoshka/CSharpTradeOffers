using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic config file
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The meat of the config
        /// </summary>
        public RootConfig Cfg = new RootConfig();
        /// <summary>
        /// Dictionary forms of the lists contained in for easy access/use
        /// </summary>
        public Dictionaries ConfigDictionaries = new Dictionaries();
        /// <summary>
        /// I forgot why I put this here but it's probably important.
        /// I forgot or it's obvious. TODO: Add better documentation
        /// </summary>
        public string MarketEligibilityJson { get; set; }
        private readonly string _path;

        /// <summary>
        /// Initializes the Config and the path to use
        /// </summary>
        /// <param name="path"></param>
        public Config(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public void Reload()
        {
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();

                #region append
                // BuildMyString.com generated code. Please enjoy your string responsibly.

                var sb = new StringBuilder();

                sb.Append("{\r\n");
                sb.Append("    \"Username\": \"null\",\r\n");
                sb.Append("    \"Password\": \"null\",\r\n");
                sb.Append("    \"SteamMachineAuth\": \"null\",\r\n");
                sb.Append("    \"Api_Key\": \"null\",\r\n");
                sb.Append("    \"Greet_Message\": \"Nada\",\r\n");
                sb.Append("    \"Add_Officers\": true,\r\n");
                sb.Append("    \"Verbose\": true,\r\n");
                sb.Append("    \"Log_Chat\": false,\r\n");
                sb.Append("    \"Log\": true,\r\n");
                sb.Append("    \"Officers\": [{\r\n");
                sb.Append("        \"steamid\": 76561198060315636,\r\n");
                sb.Append("        \"permission_level\":4\r\n");
                sb.Append("    }],\r\n");
                sb.Append("    \"Command_Permissions\": [{\r\n");
                sb.Append("        \"command_name\": \"changename\",\r\n");
                sb.Append("        \"permission_level\": 4\r\n");
                sb.Append("    }],\r\n");
                sb.Append("    \"Inventories\":[440,730],\r\n");
                sb.Append("    \"Banned_Users\": []\r\n");
                sb.Append("}\r\n");
                #endregion

                File.WriteAllText(_path, sb.ToString());

                ConfigDictionaries.Officers_Dict.Clear();
                ConfigDictionaries.Command_Permissions_Dict.Clear();

                foreach (var kvp in Cfg.Officers.Select(officer => officer.ToKeyValuePair()))
                {
                    ConfigDictionaries.Officers_Dict.Add(kvp.Key, kvp.Value);
                }
                foreach (var kvp in Cfg.CommandPermissions.Select(permission => permission.ToKeyValuePair()))
                {
                    ConfigDictionaries.Command_Permissions_Dict.Add(kvp.Key, kvp.Value);
                }
            }

            Cfg = JsonConvert.DeserializeObject<RootConfig>(File.ReadAllText(_path)); // js.Deserialize<RootConfig>(File.ReadAllText(_path));
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(RootConfig towrite)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(towrite));
        }

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

        /// <summary>
        /// Generic command permission class.
        /// </summary>
        public class Command_Permission
        {
            /// <summary>
            /// The exact name of the command.
            /// </summary>
            public string command_name { get; set; }
            /// <summary>
            /// An integer representing a permission level.
            /// </summary>
            public int permission_level { get; set; }
            /// <summary>
            /// Turns the Command_Permission into a KeyValuePair
            /// </summary>
            /// <returns>KeyValuePair</returns>
            public KeyValuePair<string, int> ToKeyValuePair()
            {
                return new KeyValuePair<string, int>(command_name, permission_level);
            }
        }

        /// <summary>
        /// A generic RootConfig object containing configuration information.
        /// </summary>
        [Newtonsoft.Json.JsonObject(Title = "RootObject")]
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
            public List<UInt32> Inventories { get; set; } 
            /// <summary>
            /// Define users that are not allowed to use this bot.
            /// </summary>
            public List<ulong> BannedUsers { get; set; }
        }

        /// <summary>
        /// Class containing Dictionary versions of Offer/CommandPermission
        /// </summary>
        public class Dictionaries
        {
            /// <summary>
            /// 
            /// </summary>
            public Dictionary<ulong, int> Officers_Dict = new Dictionary<UInt64, int>();
            /// <summary>
            /// 
            /// </summary>
            public Dictionary<string, int> Command_Permissions_Dict = new Dictionary<string, int>();
            /// <summary>
            /// Clear officers dictionary. This should not be called directly.
            /// </summary>
            public void ClearOfficer()
            {
                Officers_Dict.Clear();
            }
            /// <summary>
            /// Clear permissions dictionary. This shold not be called directly.
            /// </summary>
            public void ClearPerms()
            {
                Command_Permissions_Dict.Clear();
            }
        }
    }
}
