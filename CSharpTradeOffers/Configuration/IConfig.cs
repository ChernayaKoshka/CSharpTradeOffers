namespace CSharpTradeOffers.Configuration
{
    public interface IConfig
    {
        RootConfig Reload();
        void WriteChanges(RootConfig toWrite);
    }
}
