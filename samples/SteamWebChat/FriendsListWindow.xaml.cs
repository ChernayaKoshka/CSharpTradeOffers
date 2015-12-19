using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CSharpTradeOffers;
using CSharpTradeOffers.Web;
using CSharpTradeOffers.Community;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Trading;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FriendsListWindow
    {
        private static Account _account;
        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        public static List<Friend> Friends = new List<Friend>();
        public static List<PlayerSummary> FriendSummaries = new List<PlayerSummary>();
        public static PlayerSummary MySummary;

        public static SteamChatHandler ChatHandler;
        public static SteamUserHandler SteamUserHandler;
        public static ChatEventsManager ChatEventsManager;

        public static ChatWindow ChatWindow;

        public FriendsListWindow()
        {
            var loadingScreen = new LoadingScreen();
            loadingScreen.Show();

            #region config
            var config = ConfigHandler.Reload();
            if (string.IsNullOrEmpty(config.ApiKey))
                ComplainQuit("API key is missing. Please fill in the API key field in \"configuration.xml\"");
            if (string.IsNullOrEmpty(config.Username) || string.IsNullOrEmpty(config.Password))
                ComplainQuit(
                    "Username or Password missing. Please fill in their respective fields in \"configuration.xml\"");
            #endregion
            #region login
            _account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, config.Username, config.Password, config.SteamMachineAuth);

            if (!string.IsNullOrEmpty(_account.SteamMachineAuth))
            {
                config.SteamMachineAuth = _account.SteamMachineAuth;
                ConfigHandler.WriteChanges(config);
            }
            #endregion
            #region activate apis
            ChatHandler = new SteamChatHandler(_account);
            SteamUserHandler = new SteamUserHandler(config.ApiKey);
            ChatEventsManager = new ChatEventsManager(ChatHandler, TimeSpan.FromSeconds(2));
            #endregion
            #region populate lists
            MySummary = SteamUserHandler.GetPlayerSummariesV2(new List<ulong> { _account.SteamId }).FirstOrDefault();
            if (MySummary == null) throw new Exception("Unable to get my own player summary, please try again.");
            Friends = SteamUserHandler.GetFriendList(_account.SteamId, "friend");
            FriendSummaries = SteamUserHandler.GetPlayerSummariesV2(Friends.Select(x => x.SteamId).ToList());
            #endregion
            #region go online
            PollResponse response;
            Message responseMessage = null;
            do
            {
                response = ChatHandler.Poll();
                if (response.Messages == null) continue;
                responseMessage = response.Messages.FirstOrDefault(x => x.AccountIdFrom == IdConversions.UlongToAccountId(_account.SteamId));
                Thread.Sleep(TimeSpan.FromSeconds(2));
            } while ((response.Error != "OK" || responseMessage?.PersonaState == 0));
            #endregion

            loadingScreen.Close();

            ChatEventsManager.ChatMessageReceived += OnMessage;

            InitializeComponent();
            Loaded += FriendsListWindow_Loaded;
            Closed += FriendsListWindow_Closed;
        }

        void FriendsListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var control in FriendSummaries.OrderBy(x => x.PersonaName).Select(friend => new FriendControl
            {
                personaName = { Content = friend.PersonaName },
                avatarImage = { Source = new BitmapImage(new Uri(friend.AvatarMedium)) }
            }))
            {
                control.MouseDoubleClick += FriendItem_Clicked;
                friendsStackPanel.Children.Add(control);
            }
        }

        static void FriendsListWindow_Closed(object sender, EventArgs e)
        {
            if(ChatWindow == null) return;
            ChatWindow.Polling = false;
            ChatWindow.Close();
        }

        void OnMessage(object sender, ChatMessageArgs e)
        {
            CheckCreateChatWindow();

            ChatWindow.AddChatWindow(
                FriendSummaries.FirstOrDefault(
                    x => x.SteamId == IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom)), e.ChatMessage.Text);
        }

        void CheckCreateChatWindow()
        {
            if (ChatWindow != null) return;

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                {
                    ChatWindow = new ChatWindow(ChatEventsManager, ChatHandler, SteamUserHandler);
                    ChatWindow.Closed += ChatWindow_Closed;
                    ChatWindow.Show();
                });
            }
            else
            {
                ChatWindow = new ChatWindow(ChatEventsManager, ChatHandler, SteamUserHandler);
                ChatWindow.Closed += ChatWindow_Closed;
                ChatWindow.Show();
            }
        }

        static void ChatWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as ChatWindow;
            if (window == null) return;
            window.Polling = false;
            ChatWindow = null;
        }

        void FriendItem_Clicked(object sender, MouseButtonEventArgs e)
        {
            var control = sender as FriendControl;
            if (control == null) return;

            CheckCreateChatWindow();

            ChatWindow.AddChatWindow(FriendSummaries.FirstOrDefault(x => x.PersonaName == (string)control.personaName.Content));
        }

        void ComplainQuit(string message, string title = "Error")
        {
            MessageBox.Show(message, title);
            Close();
        }
    }
}
