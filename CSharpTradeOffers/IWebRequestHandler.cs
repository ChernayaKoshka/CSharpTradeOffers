using System.Collections.Generic;
using System.Net;

namespace CSharpTradeOffers
{
    /// <summary>
    /// A generic Request Handler from the web.
    /// </summary>
    /// <typeparam name="TResponse">The response type to get</typeparam>
    public interface IWebRequestHandler<out TResponse> where TResponse : IResponse
    {
        /// <summary>
        /// Send a request to a URL and receives a response of type <see cref="IResponse" />.
        /// </summary>
        /// <returns>Return's a generic <see cref="IResponse" /> from the request.</returns>
        TResponse HandleWebRequest(string url, string method, Dictionary<string, string> data,
            CookieContainer cookies, bool xHeaders, string referer);
    }
}