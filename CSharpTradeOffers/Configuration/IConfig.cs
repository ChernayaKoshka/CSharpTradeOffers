namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// IConfig interface so the default ConfigHandler that
    /// uses JSON does not have to be used.
    /// </summary>
    public interface IConfigHandler
    {
        DefaultConfig Reload();
        void WriteChanges(DefaultConfig toWrite);
    }
}
