using System;
using System.Net;

namespace CSharpTradeOffers
{
    public interface IResponse : IDisposable
    {
        ISteamStream GetResponseStream();
        CookieCollection Cookies { get; }
    }
}