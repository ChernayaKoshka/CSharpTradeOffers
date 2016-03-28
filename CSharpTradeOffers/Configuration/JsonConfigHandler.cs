using System.IO;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic config file handler
    /// </summary>
    public class JsonConfigHandler
    {
        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public DefaultConfig Reload(DefaultConfig config)
        {
            if (!File.Exists(config.Path))
            {
                File.WriteAllText(config.Path, config.SerializeToJson());
            }

            config = JsonConvert.DeserializeObject<DefaultConfig>(File.ReadAllText(config.Path));

            return config;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(DefaultConfig towrite)
        {
            File.WriteAllText(towrite.Path, JsonConvert.SerializeObject(towrite));
        }
    }
}
