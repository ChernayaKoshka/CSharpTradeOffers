using System.IO;

namespace CSharpTradeOffers
{
    public interface ISteamStream
    {
        string ReadToEnd();
        Stream GetUnderlyingStream();
    }
}