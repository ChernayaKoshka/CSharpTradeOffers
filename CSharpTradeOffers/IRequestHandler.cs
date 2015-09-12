using System.Collections.Generic;
using System.Net;

namespace CSharpTradeOffers
{
    public interface IRequestHandler<out TResponse> where TResponse : IResponse
    {
        TResponse HandleRequest(string url, string method, Dictionary<string, string> data,
            CookieContainer cookies, bool xHeaders, string referer);
    }
}