using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic config file handler
    /// </summary>
    public class JsonConfigHandler : IConfig
    {
        private readonly string _path;

        /// <summary>
        /// Initializes the Config and the path to use
        /// </summary>
        /// <param name="path">Path of the config file, will be created if it does not exist.</param>
        public JsonConfigHandler(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public Config Reload()
        {
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();

                #region append

                // BuildMyString.com generated code. Please enjoy your string responsibly.

                var sb = new StringBuilder();

                sb.Append("{\r\n");
                sb.Append("    \"Username\": \"\",\r\n");
                sb.Append("    \"Password\": \"\",\r\n");
                sb.Append("    \"ApiKey\": \"\",\r\n");
                sb.Append("    \"SteamMachineAuth\": \"\",\r\n");
                sb.Append("    \"Inventories\":[440,730]\r\n");
                sb.Append("}\r\n");

                #endregion

                File.WriteAllText(_path, sb.ToString());
            }

            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_path));

            return config;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(Config towrite)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(towrite));
        }
    }
}
