namespace CSharpTradeOffers
{
    /// <summary>
    /// A response stream.
    /// </summary>
    public interface IResponseStream
    {
        /// <summary>
        /// Read the contents of the Stream.
        /// </summary>
        /// <returns>The stream contents.</returns>
        string ReadStream();

        /// <summary>
        /// Deserializes the stream to a serializable type.
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <returns>The deserialized type.</returns>
        TSerializable Deserialize<TSerializable>();
    }
}