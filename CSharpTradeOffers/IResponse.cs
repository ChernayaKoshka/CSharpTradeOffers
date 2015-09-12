using System;
using System.IO;
using System.Net;

namespace CSharpTradeOffers
{
    public interface IResponse : IDisposable
    {
        Stream GetResponseStream();
        CookieCollection Cookies { get; }
    }
}