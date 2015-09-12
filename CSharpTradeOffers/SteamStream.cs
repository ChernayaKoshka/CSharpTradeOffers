using System.IO;

namespace CSharpTradeOffers
{
    internal sealed class SteamStream : ISteamStream
    {
        private readonly Stream _stream;

        public SteamStream(Stream stream)
        {
            _stream = stream;
        }

        public string ReadToEnd()
        {
            using (StreamReader streamReader = new StreamReader(_stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public Stream GetUnderlyingStream()
        {
            // TODO: We don't really want to expose the Stream, as it defeats the point of having this SteamStream facade. 
            // TODO: (cont.) It will make unit testing harder later on. But at the moment, some classes rely on this.
            return _stream;
        }
    }
}