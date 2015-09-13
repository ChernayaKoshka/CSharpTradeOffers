using System;
using System.Net;

namespace CSharpTradeOffers
{
    /// <summary>
    /// A response from the web.
    /// </summary>
    public interface IResponse : IDisposable
    {
        /// <summary>
        /// The stream created from the response.
        /// </summary>
        /// <returns></returns>
        IResponseStream GetResponseStream();

        /// <summary>
        /// Any cookies created from the response.
        /// </summary>
        CookieCollection Cookies { get; }
    }
}