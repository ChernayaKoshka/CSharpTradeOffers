using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace DonationBot
{
    class Config
    {
        public Rootobject config = new Rootobject();

        public void Reload()
        {
            if (!File.Exists("config.cfg"))
                Generate();

            var jss = new JavaScriptSerializer();
            config = jss.Deserialize<Rootobject>(File.ReadAllText("config.cfg"));
        }

        public void WriteChanges()
        {
            if (!File.Exists("config.cfg"))
                Generate();
            var json = new JavaScriptSerializer().Serialize(config);
            File.WriteAllText("config.cfg", json);
        }

        private static void Generate()
        {
            // BuildMyString.com generated code. Please enjoy your string responsibly.

            var sb = new StringBuilder();

            sb.Append("{\r\n");
            sb.Append("  \"username\":null,\r\n");
            sb.Append("  \"password\":null,\r\n");
            sb.Append("  \"apikey\":null,\r\n");
            sb.Append("  \"steamMachineAuth\":null\r\n");
            sb.Append("}\r\n");

            File.WriteAllText("config.cfg", sb.ToString());
        }
    }


    public class Rootobject
    {
        public string username { get; set; }
        public string password { get; set; }
        public string apikey { get; set; }
        public string steamMachineAuth { get; set; }
    }

}
