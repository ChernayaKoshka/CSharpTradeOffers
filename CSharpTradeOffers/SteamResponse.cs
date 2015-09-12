using System.Net;

namespace CSharpTradeOffers
{
    public sealed class SteamResponse : IResponse
    {
        private readonly HttpWebResponse _httpWebResponse;

        public SteamResponse(HttpWebResponse httpWebResponse)
        {
            _httpWebResponse = httpWebResponse;
        }

        public CookieCollection Cookies => _httpWebResponse.Cookies;

        public ISteamStream GetResponseStream()
        {
            return new SteamStream(_httpWebResponse.GetResponseStream());
        }

        public void Dispose()
        {
            _httpWebResponse.Dispose();
        }
    }
}