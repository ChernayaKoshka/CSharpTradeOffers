using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Handles requests to the "www.SteamCommunity.com" page.
    /// </summary>
    public class SteamWebRequestHandler : IWebRequestHandler<SteamResponse>
    {
        /// <summary>
        /// Sends a request to a url, refered from the steam community. Returns a steam response.
        /// </summary>
        /// <returns>A stream response</returns>
        public SteamResponse HandleWebRequest(string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            bool isGetMethod = (method.ToLower() == "get");
            string dataString = null;

            if (data != null)
                dataString = "?" + DictionaryToUrlString(data);

            if (isGetMethod && !string.IsNullOrEmpty(dataString))
            {
                url += dataString;
            }

            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Method = method;
                request.Accept = "application/json, text/javascript;q=0.9, */*;q=0.5";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
                request.Referer = string.IsNullOrEmpty(referer) ? "http://steamcommunity.com/" : referer;
                request.Timeout = 50000;
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                if (xHeaders)
                {
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    request.Headers.Add("X-Prototype-Version", "1.7");
                }
                request.CookieContainer = cookies ?? new CookieContainer();

                if (data != null && !isGetMethod)
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(DictionaryToUrlString(data));
                    request.ContentLength = dataBytes.Length;

                    using (var reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(dataBytes, 0, dataBytes.Length);
                    }
                }

                return new SteamResponse(request.GetResponse() as HttpWebResponse);
            }
            return null;
        }

        /// <summary>
        /// Converts a dictionary to URL parameters. Ex: ?param=arg
        /// </summary>
        /// <param name="dict">The Dictionary to be converted.</param>
        /// <returns>A concatenated string of URL arguments.</returns>
        private static string DictionaryToUrlString(Dictionary<string, string> dict)
        {
            string joined = "";
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                if (joined != "")
                    joined += "&";

                joined += $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}";
            }
            return joined;
        }
    }
}