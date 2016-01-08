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
        /// <returns>The stream as a string</returns>
        string ReadStream();

        /// <summary>
        /// Deserializes the stream to an XML serializable type.
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        TSerializable DeserializeXml<TSerializable>();

        /// <summary>
        /// Deserializes the stream to a JSON serializable type
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        TSerializable DeserializeJson<TSerializable>();

        /// <summary>
        /// Any cookies created from the response.
        /// </summary>
        CookieCollection Cookies { get; }
    }
}