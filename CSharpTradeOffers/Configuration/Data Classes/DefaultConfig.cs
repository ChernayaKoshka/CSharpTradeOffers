namespace CSharpTradeOffers.Configuration
{
    public class DefaultConfig : ConfigFrame
    {
        public DefaultConfig()
        {
            base.Path = "default_configname.cfg";
        }

        public DefaultConfig(string path) : base(path)
        {
            InitializeAll();
        }

        public ConfigErrorResult CheckForErrors()
        {
            var result = new ConfigErrorResult(true);

            if (string.IsNullOrWhiteSpace(Username))
               result.AddError("<Username></Username> tag is empty.");

            if (string.IsNullOrWhiteSpace(Password))
                result.AddError("<Password></Password> tag is empty.");

            if (string.IsNullOrWhiteSpace(ApiKey))
                result.AddError("<ApiKey></ApiKey> tag is empty. An API key can be retrieved from https://www.steamcommunity.com/dev/apikey");

            return result;
        }

        public sealed override void InitializeAll()
        {
            Username = " ";
            Password = " ";
            SteamMachineAuth = " ";
            ApiKey = " ";
        }
    }
}
