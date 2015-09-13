using System.IO;
using System.Xml.Serialization;

namespace CSharpTradeOffers
{
    /// <summary>
    /// A facade for a <see cref="Stream" /> from a Steam web request.
    /// </summary>
    internal sealed class SteamStream : IResponseStream
    {
        private readonly Stream _stream;

        /// <summary>
        /// Encapsulate a stream from a Steam Request.
        /// </summary>
        /// <param name="stream">The underlying steam stream.</param>
        public SteamStream(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Read the steam stream to the end.
        /// </summary>
        /// <returns>The stream contents.</returns>
        public string ReadStream()
        {
            using (StreamReader streamReader = new StreamReader(_stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Deserializes the stream to a serializable type with Xml.
        /// </summary>
        /// <typeparam name="TSerializable">An XML serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        public TSerializable Deserialize<TSerializable>()
        {
            return (TSerializable) (new XmlSerializer(typeof (TSerializable)).Deserialize(_stream));
        }
    }
}