using System.IO;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Configuration
{
    public class GenericConfigHandler
    {
        private readonly string _path;

        /// <summary>
        /// Initializes the Config and the path to use
        /// </summary>
        /// <param name="path"></param>
        public GenericConfigHandler(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reloads the configuration file (path). If file is not present, it will generate a new one.
        /// </summary>
        /// <returns>A generic object where TConfig:IConfig.</returns>
        public TConfig Reload<TConfig>(TConfig config)
        {
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();
                File.WriteAllText(_path, config.SerializeToXml());
            }

            using (var sr = new StreamReader(_path))
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
        public void WriteChanges<TConfig>(TConfig towrite)
        {
            File.WriteAllText(_path, towrite.SerializeToXml());
        }
    }
}
