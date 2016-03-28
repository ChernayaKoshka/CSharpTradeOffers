using System.IO;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Configuration
{
    public class GenericConfigHandler
    {
        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A generic object where TConfig:IConfig.</returns>
        public TConfig Reload<TConfig>(TConfig config) where TConfig : IConfig
        {
            if (!File.Exists(config.Path))
            {
                File.Create(config.Path).Close();
                File.WriteAllText(config.Path, config.SerializeToXml());
            }

            using (var sr = new StreamReader(config.Path))
            {
                config =
                    (TConfig)
                        new XmlSerializer(typeof(TConfig)).Deserialize(sr);
            }

            return config;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges<TConfig>(TConfig towrite) where TConfig : IConfig
        {
            File.WriteAllText(towrite.Path, towrite.SerializeToXml());
        }
    }
}
