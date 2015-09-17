using System;
using System.Net;

namespace CSharpTradeOffers.Web
{
    /// <summary>
    /// A response from the web.
    /// </summary>
    public interface IResponse : IDisposable
    {
        /// <summary>
        /// Reads the stream to a string 
        /// </summary>
        /// <returns></returns>
        string ReadStream();

        /// <summary>
        /// Deserializes the stream to a serializable type.
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        TSerializable Deserialize<TSerializable>();

        /// <summary>
        /// Any cookies created from the response.
        /// </summary>
        CookieCollection Cookies { get; }
    }
}