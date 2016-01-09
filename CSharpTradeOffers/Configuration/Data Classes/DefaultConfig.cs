namespace CSharpTradeOffers.Configuration
{
    public class DefaultConfig : ConfigFrame
    {
        public DefaultConfig()
        {
            InitializeAll();
        }

        public override sealed void InitializeAll()
        {
            Username = " ";
            Password = " ";
            SteamMachineAuth = " ";
            ApiKey = " ";
        }
    }
}
