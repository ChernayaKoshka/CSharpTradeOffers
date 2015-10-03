using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CSharpTradeOffers.Web;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class SteamChatHandler
    {
        private readonly Web.Web _web = new Web.Web(new SteamWebRequestHandler());
        private readonly Account _account;

        private const string BaseAuthUrl = "https://api.steampowered.com/ISteamWebUserPresenceOAuth/";
        private const string BaseChatUrl = "https://steamcommunity.com/chat/";

        private readonly WebPresenceOAuthLogonResponse _auth;
        private readonly string _basejQuery;
        private const string AccessToken = "901ee4203d44b369fceabe2da9b4c88d";

        private int _pollId = 1;

        /// <summary>
        /// Creates a new SteamChatHandler. Generates a random jQueryId to use.
        /// </summary>
        /// <param name="account">Account object of the account to use.</param>
        public SteamChatHandler(Account account)
        {
            _account = account;

            var rand = new Random();
            var jQueryId = (long)(((rand.NextDouble() * 2.0 - 1.0) * long.MaxValue) % 99999999999); //might be able to be larger, haven't checked
            if (jQueryId < 0) jQueryId = -jQueryId;
            _basejQuery = "jQuery" + jQueryId + _account.SteamId + "_{0}";

            _auth = Logon();
        }

        /// <summary>
        /// Logs on to Steam community chat.
        /// </summary>
        /// <returns>WebPresenceOAuthLogonResponse</returns>
        private WebPresenceOAuthLogonResponse Logon()
        {
            const string url = BaseAuthUrl + "Logon/v0001/";
            string jQuery = string.Format(_basejQuery, UnixTimeNow());
            var data = new Dictionary<string, string>
            {
                {"jsonp", jQuery},
                {"ui_mode", "web"},
                {"access_token", AccessToken} //public realm iirc
            };

            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, false).ReadStream(); //returns an annoying JSON string that can't quite be deserialized yet
            response = StripjQueryArtifacts(response); //remove /**/jQuery11110010656769154593349_1442204142816( and remove )
            return JsonConvert.DeserializeObject<WebPresenceOAuthLogonResponse>(response);
        }
        
        /// <summary>
        /// Pretty sure this function does persona states (among other things?)
        /// Message 29 = Go online
        /// </summary>
        /// <param name="message">
        /// Message numbers that I know:
        /// 29 = poll me
        /// 30 = poll friends (?)
        /// 35 = poll new messages</param>
        /// <param name="secTimeOut">Seconds until a timeout event occurs?</param>
        /// <param name="secIdleTime">Seconds idle, I assume Steam uses this to set your state to "away"</param>
        /// <param name="useAccountIds">Probably always true.</param>
        /// <returns>PollResponse object</returns>
        public PollResponse Poll(int message,int secTimeOut,int secIdleTime,bool useAccountIds = true)
        {
            const string url = BaseAuthUrl + "Poll/v0001/";
            string jQuery = string.Format(_basejQuery, UnixTimeNow());
            var data = new Dictionary<string, string>
            {
                {"jsonp", jQuery},
                {"umqid", _auth.UmqId},
                {"message", message.ToString()},
                {"pollid", _pollId.ToString()},
                {"sectimeout", secTimeOut.ToString()},
                {"secidletime", secIdleTime.ToString()},
                {"use_accountids", useAccountIds.IntValue().ToString()},
                {"access_token", AccessToken}
            };

            string response = _web.Fetch(url, "GET", data, xHeaders: false).ReadStream();
            _pollId++;
            response = StripjQueryArtifacts(response);
            return JsonConvert.DeserializeObject<PollResponse>(response);
        }

        static string StripjQueryArtifacts(string toStrip)
        {
            toStrip = toStrip.Remove(0, toStrip.IndexOf("(", StringComparison.Ordinal) + 1);
            return toStrip.Substring(0, toStrip.Length - 1);
        }

        /// <summary>
        /// Gets the Unix time. Also known as Epoch.
        /// </summary>
        /// <returns>A long integer of the number of seconds since 1,1,1970</returns>
        static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Gets the state of a friend.
        /// </summary>
        /// <param name="accountId">AccountId of friend.</param>
        /// <returns>A FriendStateResponse object.</returns>
        public FriendStateResponse FriendState(uint accountId)
        {
            string url = BaseChatUrl + "friendstate/" + accountId;
            return
                JsonConvert.DeserializeObject<FriendStateResponse>(_web.Fetch(url, "GET", null, _account.AuthContainer).ReadStream());
        }

        /// <summary>
        /// Request chat log of conversation.
        /// </summary>
        /// <param name="accountId">AccountId of the person to request the chat log of.</param>
        /// <returns>A List of ChatLogMessage objects.</returns>
        public List<ChatLogMessage> ChatLog(uint accountId)
        {
            string url = BaseChatUrl + "chatlog/" + accountId;
            string sessionid = (from Cookie cookie in _account.AuthContainer.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();
            var data = new Dictionary<string, string> {{"sessionid", sessionid}};

            return
                JsonConvert.DeserializeObject<List<ChatLogMessage>>(_web.Fetch(url, "GET", data, _account.AuthContainer).ReadStream());
        } 

        //saytext = send chat message
        //there are others but I don't know them yet
        /// <summary>
        /// Sends a message to Steam
        /// </summary>
        /// <param name="sendTo">The SteamId64 of the person to send the message to.</param>
        /// <param name="type">Message type. (saytext = send message, others but idk)</param>
        /// <param name="message">Message to send</param>
        /// <returns>SendChatMessageResponse</returns>
        public SendChatMessageResponse Message(ulong sendTo, string type, string message)
        {
            const string url = BaseAuthUrl + "Message/v0001/";
            string jQuery = string.Format(_basejQuery, UnixTimeNow());
            var data = new Dictionary<string, string>
            {
                {"jsonp", jQuery},
                {"umqid", _auth.UmqId},
                {"type", type},
                {"steamid_dst", sendTo.ToString()},
                {"text", message},
                {"access_token", AccessToken}
            };

            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, xHeaders: false).ReadStream();
            response = StripjQueryArtifacts(response);
            return JsonConvert.DeserializeObject<SendChatMessageResponse>(response);
        }

    }
}
