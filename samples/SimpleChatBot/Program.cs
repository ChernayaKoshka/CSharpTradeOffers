using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTradeOffers;
using CSharpTradeOffers.Community;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Web;

namespace SimpleChatBot
{
    class Program
    {
        private static Account _account;
        private static DefaultConfig _config = new DefaultConfig();
        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        public static SteamChatHandler ChatHandler { get; set; }
        public static ChatEventsManager ChatEventsManager { get; set; }

        public static SteamUserHandler SteamUserHandler;

        private static void Main()
        {
            Console.Title = "Chat Bot by sne4kyFox";
            Console.WriteLine("Welcome to the chat bot!");
            Console.WriteLine(
                "By using this software you agree to the terms in \"license.txt\".");

            LoadConfig();
            Login();

            SteamUserHandler = new SteamUserHandler(_config.ApiKey);

            //handles sending messages and such
            ChatHandler = new SteamChatHandler(_account);
            //allows you to use a built-in message loop without constructing your own. Non-blocking
            ChatEventsManager = new ChatEventsManager(ChatHandler);
            //non-blocking callback like in SteamKit2
            ChatEventsManager.ChatMessageReceived += OnChatMessage;
        }

        //load config
        static void LoadConfig()
        {
            _config = ConfigHandler.Reload();

            if (!string.IsNullOrEmpty(_config.ApiKey)) return;
            Console.WriteLine("Fatal error: API key is missing. Please fill in the API key field in \"configuration.xml\"");
            Console.ReadLine();
            Environment.Exit(-1);
        }

        //chat message event fires whenever we receive an actual message
        private static void OnChatMessage(object sender, ChatMessageArgs e)
        {
            switch (e.ChatMessage.Text.ToLower())
            {
                case "!utc":
                    ChatHandler.Message(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom), "saytext",
                        "The current UTC timestamp is: " + e.ChatMessage.UtcTimestamp);
                    return;
                case "!timestamp":
                    ChatHandler.Message(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom), "saytext",
                        "The current timestamp is: " + e.ChatMessage.Timestamp);
                    return;
                case "!myfriendcount":
                    List<Friend> friends = SteamUserHandler.GetFriendList(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom), "friend");
                    if (friends == null) throw new ArgumentNullException(nameof(friends));
                    ChatHandler.Message(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom), "saytext",
                        "You have " + friends.Count + " friends.");
                    return;
                case "!logoff":
                    ChatEventsManager.EndMessageLoop();
                    ChatHandler.Logoff();
                    return;
                //more commands...
            }

            //actual messages etc...
            ChatHandler.Message(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom), "saytext",
                "Hello!");
        }

        //login
        static void Login()
        {
            Console.WriteLine("Attempting web login...");

            _account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, _config.Username, _config.Password, _config.SteamMachineAuth);

            if (!string.IsNullOrEmpty(_account.SteamMachineAuth))
            {
                _config.SteamMachineAuth = _account.SteamMachineAuth;
                ConfigHandler.WriteChanges(_config);
            }

            Console.WriteLine("Login was successful!");
        }
    }
}
