using System.IO;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Configuration
{
    public class XmlConfigHandler
    {
        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A RootConfig object.</returns>
        public DefaultConfig Reload(DefaultConfig config)
        {
            DefaultConfig defaultConfig = new DefaultConfig(config.Path);

            if (!File.Exists(config.Path))
            {
                File.WriteAllText(config.Path, defaultConfig.SerializeToXml());
            }

            using (var sr = new StreamReader(config.Path))
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
            File.WriteAllText(towrite.Path, towrite.SerializeToXml());
        }
    }
}
