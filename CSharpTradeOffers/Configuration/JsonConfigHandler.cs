using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic config file handler
    /// </summary>
    public class JsonConfigHandlerHandler : IConfigHandler
    {
        private readonly string _path;

        /// <summary>
        /// Initializes the Config and the path to use
        /// </summary>
        /// <param name="path">Path of the config file, will be created if it does not exist.</param>
        public JsonConfigHandlerHandler(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public DefaultConfig Reload()
        {
            var config = new DefaultConfig();
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, config.SerializeToJson());
            }

            config = JsonConvert.DeserializeObject<DefaultConfig>(File.ReadAllText(_path));

            return config;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(DefaultConfig towrite)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(towrite));
        }
    }
}
