using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    public class SteamChatHandler
    {
        private readonly Web _web = new Web(new SteamWebRequestHandler());
        private readonly Account _account;

        private const string BaseAuthUrl = "https://api.steampowered.com/ISteamWebUserPresenceOAuth/";
        private const string BaseChatUrl = "https://steamcommunity.com/chat/";

        private readonly WebPresenceOAuthLogonResponse _auth;
        private readonly string _basejQuery;
        private const string AccessToken = "901ee4203d44b369fceabe2da9b4c88d";

        private int _pollId = 1;

        public SteamChatHandler(Account account)
        {
            _account = account;

            var rand = new Random();
            var jQueryId = (long)(((rand.NextDouble() * 2.0 - 1.0) * long.MaxValue) % 99999999999); //might be able to be larger, haven't checked
            if (jQueryId < 0) jQueryId = -jQueryId;
            _basejQuery = "jQuery" + jQueryId + _account.SteamId + "_{0}";

            _auth = Logon();
        }

        /*https://api.steampowered.com/
            ISteamWebUserPresenceOAuth/
            Logon/
            v0001/
            ?jsonp=
            jQuery11110010656769154593349_1442204142816
            &ui_mode=web
            &access_token=901ee4203d44b369fceabe2da9b4c88d
            &_=1442204142817*/

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

            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, false); //returns an annoying JSON string that can't quite be deserialized yet
            response = StripjQueryArtifacts(response); //remove /**/jQuery11110010656769154593349_1442204142816( and remove )
            return JsonConvert.DeserializeObject<WebPresenceOAuthLogonResponse>(response);
        }

        /*https://api.steampowered.com/ISteamWebUserPresenceOAuth/
        Poll/
        v0001/
        ?jsonp=jQuery11110010656769154593349_1442204142816
        &umqid=6194219628342327081
        &message=29
        &pollid=1
        &sectimeout=20
        &secidletime=0
        &use_accountids=1
        &access_token=901ee4203d44b369fceabe2da9b4c88d
        */
        /*
        Message numbers that I know:
        29 = poll me
        30 = poll friends
        35 = poll new messages
        */
        public PollResponse Poll(int message,int secTimeOut,int secIdleTime,bool useAccountIds)
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

            string response = _web.Fetch(url, "GET", data, xHeaders: false);
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

        public FriendStateResponse FriendState(uint accountId)
        {
            string url = BaseChatUrl + "friendstate/" + accountId;
            return
                JsonConvert.DeserializeObject<FriendStateResponse>(_web.Fetch(url, "GET", null, _account.AuthContainer));
        }

        public List<ChatLogMessage> ChatLog(uint accountId)
        {
            string url = BaseChatUrl + "chatlog/" + accountId;
            string sessionid = (from Cookie cookie in _account.AuthContainer.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();
            var data = new Dictionary<string, string> {{"sessionid", sessionid}};

            return
                JsonConvert.DeserializeObject<List<ChatLogMessage>>(_web.Fetch(url, "GET", data, _account.AuthContainer));
        } 

        //saytext = send chat message
        //there are others but I don't know them yet
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

            string response = _web.Fetch(url, "GET", data, _account.AuthContainer, xHeaders: false);
            response = StripjQueryArtifacts(response);
            return JsonConvert.DeserializeObject<SendChatMessageResponse>(response);
        }

    }
}
