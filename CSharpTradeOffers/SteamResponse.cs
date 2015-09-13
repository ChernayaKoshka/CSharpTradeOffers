using System.Net;

namespace CSharpTradeOffers
{
    /// <summary>
    /// A response from Steam.
    /// </summary>
    public sealed class SteamResponse : IResponse
    {
        private readonly HttpWebResponse _httpWebResponse;

        /// <summary>
        /// Create a steam response from a web response.
        /// </summary>
        /// <param name="httpWebResponse">A Web Response from Steam.</param>
        public SteamResponse(HttpWebResponse httpWebResponse)
        {
            _httpWebResponse = httpWebResponse;
        }

        /// <summary>
        /// Cookies collected from the response.
        /// </summary>
        public CookieCollection Cookies => _httpWebResponse.Cookies;

        /// <summary>
        /// The response stream.
        /// </summary>
        /// <returns></returns>
        public IResponseStream GetResponseStream()
        {
            return new SteamStream(_httpWebResponse.GetResponseStream());
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            _httpWebResponse.Dispose();
        }
    }
}