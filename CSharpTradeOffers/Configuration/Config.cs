using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic config file
    /// </summary>
    public class Config : IConfig
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
        public RootConfig Reload()
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
            return Cfg;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(RootConfig towrite)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(towrite));
        }
    }
}
