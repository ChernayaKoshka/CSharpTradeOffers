using System.IO;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Configuration
{
    public class XmlConfigHandler : IConfigHandler
    {

        private readonly string _path;

        /// <summary>
        /// Initializes the Config and the path to use
        /// </summary>
        /// <param name="path"></param>
        public XmlConfigHandler(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public DefaultConfig Reload()
        {
            var defaultConfig = new DefaultConfig();

            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, defaultConfig.SerializeToXml());
            }

            using (var sr = new StreamReader(_path))
            {
                defaultConfig =
                    (DefaultConfig)
                        new XmlSerializer(typeof (DefaultConfig)).Deserialize(sr);
            }

            return defaultConfig;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(DefaultConfig towrite)
        {
            File.WriteAllText(_path, towrite.SerializeToXml());
        }
    }
}
