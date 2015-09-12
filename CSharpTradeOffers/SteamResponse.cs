using System.IO;
using System.Net;

namespace CSharpTradeOffers
{
    public class SteamResponse : IResponse
    {
        private readonly HttpWebResponse _httpWebResponse;

        public SteamResponse(HttpWebResponse httpWebResponse)
        {
            _httpWebResponse = httpWebResponse;
        }

        public CookieCollection Cookies => _httpWebResponse.Cookies;

        public Stream GetResponseStream()
        {
            return _httpWebResponse.GetResponseStream();
        }

        public void Dispose()
        {
            _httpWebResponse.Dispose();
        }
    }
}