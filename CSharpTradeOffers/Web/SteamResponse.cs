using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Web
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
        private Stream SteamStream => _httpWebResponse.GetResponseStream();

        /// <summary>
        /// Reads the response Stream
        /// </summary>
        /// <returns>The read Stream, however, if an exception occurs, null will be returned instead.</returns>
        public string ReadStream()
        {
            try
            {
                using (var streamReader = new StreamReader(SteamStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Deserializes the stream to a serializable type with Json.
        /// </summary>
        /// <typeparam name="TSerializable">A JSON serializable type</typeparam>
        /// <returns>The deserialized type unless stream is null, in which case it returns the default of the generic.</returns>
        public TSerializable DeserializeJson<TSerializable>()
        {
            string streamData = ReadStream();
            return streamData == null ? default(TSerializable) : JsonConvert.DeserializeObject<TSerializable>(ReadStream());
        }

        /// <summary>
        /// Deserializes the stream to a serializable type with Xml.
        /// </summary>
        /// <typeparam name="TSerializable">An XML serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        public TSerializable DeserializeXml<TSerializable>()
        {
            return (TSerializable)(new XmlSerializer(typeof(TSerializable)).Deserialize(SteamStream));
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