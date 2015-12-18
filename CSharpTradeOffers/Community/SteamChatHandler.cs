using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CSharpTradeOffers.Web;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class ChatException : Exception
    {
        public ChatException() { }

        public ChatException(string message) : base(message) { }

        public ChatException(string message, Exception inner) : base(message, inner) { }
    }

    public class MessageArgs : EventArgs
    {
        public MessageArgs(PollResponse response)
        {
            Response = response;
        }
        public PollResponse Response;
    }

    public class SteamChatHandler
    {
        private readonly Web.Web _web = new Web.Web(new SteamWebRequestHandler());
        private readonly Account _account;

        private const string BaseAuthUrl = "https://api.steampowered.com/ISteamWebUserPresenceOAuth/";
        private const string BaseChatUrl = "https://steamcommunity.com/chat/";

        private readonly WebPresenceOAuthLogonResponse _auth;
        private readonly string _basejQuery;
        private readonly string _accessToken;

        public delegate void OnMessageReceived(object sender, MessageArgs e);

        public event OnMessageReceived MessageReceived;

        private int _pollId = 1;
        private int _message = 1;
        private bool _searching;

        /// <summary>
        /// Creates a new SteamChatHandler. Generates a random jQueryId to use.
        /// </summary>
        /// <param name="account">Account object of the account to use.</param>
        public SteamChatHandler(Account account)
        {
            _account = account;

            //sloppy way to do it, but couldn't get Regex to love me
            _accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new ChatException(
                    "Error fetching access token, the account specified is not authorized to use this feature.");
            }

            var rand = new Random();                                                //6228287338999220263
            var jQueryId = (long)(((rand.NextDouble() * 2.0 - 1.0) * long.MaxValue) % 9999999999999999999); //might be able to be larger, haven't checked
            jQueryId = Math.Abs(jQueryId);
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
                {"access_token", _accessToken} //special id, like SteamId but not for some reason
            };

            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, false).ReadStream(); //returns an annoying JSON string that can't quite be deserialized yet
            response = StripjQueryArtifacts(response); //remove /**/jQuery11110010656769154593349_1442204142816( and remove )

            WebPresenceOAuthLogonResponse webPresenceOAuthLogonResponse = JsonConvert.DeserializeObject<WebPresenceOAuthLogonResponse>(response);
            _message = webPresenceOAuthLogonResponse.Message;
            return webPresenceOAuthLogonResponse;
        }

        private string GetAccessToken()
        {
            string response = _web.Fetch(BaseChatUrl, "GET", null, _account.AuthContainer).ReadStream();
            if (response.Contains("not authorized")) return null; //hideous and sloppy. :(
            string removed = response.Remove(0, response.IndexOf("\'https://api.steampowered.com/\', \"", StringComparison.Ordinal) + 34);
            return removed.Substring(0, 32);
        }

        /// <summary>
        /// Polls Steam for messages/persona states. Making a Poll also assures Steam that you are online.
        /// </summary>
        /// <param name="secTimeOut">Seconds until a timeout event occurs?</param>
        /// <param name="secIdleTime">Seconds idle, I assume Steam uses this to set your state to "away"</param>
        /// <param name="useAccountIds">Probably always true.</param>
        /// <returns>PollResponse object</returns>
        public PollResponse Poll(int secTimeOut = 0, int secIdleTime = 0, bool useAccountIds = true)
        {
            const string url = BaseAuthUrl + "Poll/v0001/";
            string jQuery = string.Format(_basejQuery, UnixTimeNow());
            var data = new Dictionary<string, string>
            {
                {"jsonp", jQuery},
                {"umqid", _auth.UmqId},
                {"message", _message.ToString()},
                {"pollid", _pollId.ToString()},
                {"sectimeout", secTimeOut.ToString()},
                {"secidletime", secIdleTime.ToString()},
                {"use_accountids", useAccountIds.IntValue().ToString()},
                {"access_token", _accessToken}
            };

            string response = _web.Fetch(url, "GET", data, xHeaders: false).ReadStream();
            _pollId++;
            response = StripjQueryArtifacts(response);
            PollResponse pollResponse = JsonConvert.DeserializeObject<PollResponse>(response);
            if (pollResponse.MessageLast != 0)
                _message = pollResponse.MessageLast;
            return pollResponse;
        }

        static string StripjQueryArtifacts(string toStrip)
        {
            toStrip = toStrip.Remove(0, toStrip.IndexOf("(", StringComparison.Ordinal) + 1);
            return toStrip.Substring(0, toStrip.Length - 1);
        }

        public void BeginMessageLoop(TimeSpan waitAfterPoll)
        {
            if (_searching) return;
            var messageThread = new Thread(() => MessageLoop(waitAfterPoll));
            messageThread.Start();
        }

        private void MessageLoop(TimeSpan waitAfterPoll)
        {
            _searching = true;
            while (_searching)
            {
                Thread.Sleep(waitAfterPoll);
                PollResponse response = Poll();
                if (response.Messages == null) continue;
                MessageReceived?.Invoke(this, new MessageArgs(response));
            }
        }

        public void EndMessageLoop()
        {
            if (_searching) _searching = false;
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
            var data = new Dictionary<string, string> { { "sessionid", sessionid } };

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
                {"access_token", _accessToken}
            };
            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, false).ReadStream();
            response = StripjQueryArtifacts(response);
            return JsonConvert.DeserializeObject<SendChatMessageResponse>(response);
        }
    }
}
