using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CSharpTradeOffers;
using CSharpTradeOffers.Community;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Web;

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

        public static List<Friend> Friends { get; private set; }
        public static List<PlayerSummary> FriendSummaries { get; private set; }
        public static PlayerSummary MySummary { get; private set; }

        public static SteamChatHandler ChatHandler { get; private set; }
        public static SteamUserHandler SteamUserHandler { get; private set; }
        public static ChatEventsManager ChatEventsManager { get; private set; }

        public static ChatWindow ChatWindow { get; private set; }

        public event LoadingFinished OnLoadingFinished;

        public delegate void LoadingFinished(object sender, EventArgs e);

        public FriendsListWindow()
        {
            InitializeComponent();
        }

        public void LoginAndGoOnline()
        {
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
                response = ChatHandler.Poll(10);
                if (response.Messages == null) continue;
                responseMessage = response.Messages.FirstOrDefault(x => x.AccountIdFrom == IdConversions.UlongToAccountId(_account.SteamId));
                Thread.Sleep(TimeSpan.FromSeconds(2));
            } while ((response.Error != "OK" || responseMessage?.PersonaState == 0));
            #endregion

            ChatEventsManager = new ChatEventsManager(ChatHandler, TimeSpan.FromSeconds(2), 10);
            ChatEventsManager.ChatMessageReceived += OnMessage;
            
            Loaded += FriendsListWindow_Loaded;
            Closed += FriendsListWindow_Closed;
            OnLoadingFinished?.Invoke(this, new EventArgs());
        }

        void FriendsListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var control
                     in from friend
                     in FriendSummaries.OrderBy(x => x.PersonaName)
                        let state = ChatHandler.FriendState(IdConversions.UlongToAccountId(friend.SteamId))
                        select new FriendControl(new ChatUser { State = state, Summary = friend }))
            {
                control.MouseDoubleClick += FriendItem_Clicked;
                friendsStackPanel.Children.Add(control);
            }
        }

        static void FriendsListWindow_Closed(object sender, EventArgs e)
        {
            ChatWindow?.Close();
            ChatEventsManager.EndMessageLoop();
            ChatHandler.Logoff();
        }

        static void ChatWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as ChatWindow;
            if (window == null) return;
            ChatWindow = null;
        }

        void FriendItem_Clicked(object sender, MouseButtonEventArgs e)
        {
            var control = sender as FriendControl;
            if (control == null) return;

            CheckCreateChatWindow();

            PlayerSummary summary = FriendSummaries.FirstOrDefault(x => x.PersonaName == control.personaName.Text);
            if (summary != null)
                ChatWindow.AddChatWindow(control.Friend, string.Empty);
        }

        void ComplainQuit(string message, string title = "Error")
        {
            MessageBox.Show(message, title);
            Close();
        }

        void OnMessage(object sender, ChatMessageArgs e)
        {
            CheckCreateChatWindow();

            PlayerSummary summary =
                FriendSummaries.FirstOrDefault(
                    x => x.SteamId == IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom));

            if (summary == null)
            {
                MessageBox.Show("I was lazy and I don't poll for new friends. They attempted to send you a message.");
                return;
            }

            List<FriendControl> friendControls = new List<FriendControl>();
            ChatUser friend = null;

            friendsStackPanel.Dispatcher.Invoke(() =>
            {
                friendControls = friendsStackPanel.Children.Cast<FriendControl>().ToList();
            });

            foreach (FriendControl control in friendControls)
            {
                ulong steamId = 0;
                control.Dispatcher.Invoke(() => { steamId = control.Friend.Summary.SteamId; });
                if (steamId != summary.SteamId) continue;
                control.Dispatcher.Invoke(() => { friend = control.Friend; });
                break;
            }

            if (friend == null) throw new Exception("Could not locate friend, please report this!");

            ChatWindow.AddChatWindow(friend, e.ChatMessage.Text);
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
    }
}
