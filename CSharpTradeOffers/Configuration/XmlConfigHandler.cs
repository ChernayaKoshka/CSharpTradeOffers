using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Configuration
{
    public class XmlConfigHandler : IConfig
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
        public Config Reload()
        {
            var config = new Config();

            if (!File.Exists(_path))
            {
                File.Create(_path).Close();

                #region append

                // BuildMyString.com generated code. Please enjoy your string responsibly.

                var sb = new StringBuilder();

                sb.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n");
                sb.Append("<Config\r\n");
                sb.Append("    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n");
                sb.Append("    xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n");
                sb.Append("    <Username></Username>\r\n");
                sb.Append("    <Password></Password>\r\n");
                sb.Append("    <ApiKey></ApiKey>\r\n");
                sb.Append("    <SteamMachineAuth></SteamMachineAuth>\r\n");
                sb.Append("    <Inventories>\r\n");
                sb.Append("        <element>440</element>\r\n");
                sb.Append("        <element>730</element>\r\n");
                sb.Append("    </Inventories>\r\n");
                sb.Append("</Config>\r\n");

                #endregion

                File.WriteAllText(_path, sb.ToString());
            }

            using (var sr = new StreamReader(_path))
            {
                config =
                    (Config)
                        new XmlSerializer(typeof (Config)).Deserialize(sr);
            }

            return config;
        }

        /// <summary>
        /// Writes the changes made to the config.
        /// </summary>
        /// <param name="towrite"></param>
        public void WriteChanges(Config towrite)
        {
            File.WriteAllText(_path, towrite.SerializeToXml());
        }
    }
}
