using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CSharpTradeOffers;
using CSharpTradeOffers.Community;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatWindow
    {
        public readonly SteamChatHandler ChatHandler;

        public static SteamUserHandler SteamUserHandler;

        public bool Polling;

        public ChatWindow(ChatEventsManager chatEventsManager, SteamChatHandler chatHandler, SteamUserHandler steamUserHandler)
        {
            var loadingScreen = new LoadingScreen();
            loadingScreen.Show();

            chatEventsManager.ChatMessageReceived += OnChatMessage;
            SteamUserHandler = steamUserHandler;
            ChatHandler = chatHandler;

            loadingScreen.Close();
            InitializeComponent();
        }

        public void AddChatWindow(PlayerSummary summary, string message = "")
        {
            InvokeAddTab(summary, message);
        }

        void OnChatMessage(object sender, ChatMessageArgs e)
        {
            ChatControl control = FindMatchingChatControl(IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom));
            if (control != null)
            {
                InvokeHandleMessage(control, e.ChatMessage.Text);
            }
            else
            {
                PlayerSummary summary =
                    SteamUserHandler.GetPlayerSummariesV2(new List<ulong>
                    {
                                        IdConversions.AccountIdToUlong(e.ChatMessage.AccountIdFrom)
                    }).FirstOrDefault();

                InvokeAddTab(summary, e.ChatMessage.Text);
            }
        }

        ChatControl FindMatchingChatControl(ulong steamId)
        {
            foreach (TabItem item in chatTabs.Items)
            {
                object itemObject = null;
                item.Dispatcher.Invoke(() => { itemObject = item.Content; });
                var control = itemObject as ChatControl;
                if (control?.Chatter.SteamId != steamId)
                    continue;
                return control;
            }
            return null;
        }

        void InvokeAddTab(PlayerSummary summary, string message)
        {
            if (summary == null) return;
            if (FindMatchingChatControl(summary.SteamId) != null) return;

            if (!chatTabs.Dispatcher.CheckAccess())
                chatTabs.Dispatcher.Invoke(() =>
                {
                    var tabItem = new TabItem { Header = summary.PersonaName };
                    var chatControl = new ChatControl(ChatHandler, summary);
                    tabItem.Content = chatControl;
                    chatTabs.Items.Add(tabItem);
                    if (!string.IsNullOrEmpty(message))
                        InvokeHandleMessage(chatControl, message);
                });
            else
            {
                var tabItem = new TabItem { Header = summary.PersonaName };
                var chatControl = new ChatControl(ChatHandler, summary);
                tabItem.Content = chatControl;
                chatTabs.Items.Add(tabItem);

                if (!string.IsNullOrEmpty(message))
                    InvokeHandleMessage(chatControl, message);
            }
        }

        void InvokeHandleMessage(ChatControl control, string message)
        {
            if (!control.Dispatcher.CheckAccess())
                control.Dispatcher.Invoke(() => { control.HandleMessage(message); });
            else
                control.HandleMessage(message);
        }
    }
}
