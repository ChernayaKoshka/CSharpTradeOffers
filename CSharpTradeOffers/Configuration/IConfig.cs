namespace CSharpTradeOffers.Configuration
{
    interface IConfig
    {
        RootConfig Reload();
        void WriteChanges(RootConfig toWrite);
    }
}
