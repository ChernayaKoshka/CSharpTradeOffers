using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CSharpTradeOffers.Community;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl
    {
        public readonly TabItem ContainingTab;
        public readonly ulong ChatterId;
        public ChatUser Friend;

        private readonly ChatWindow _window;
        private readonly SteamChatHandler _chatHandler;

        public ChatControl(ChatWindow window, TabItem containingTab, SteamChatHandler chatHandler, ChatUser friend)
        {
            Friend = friend;
            ChatterId = friend.Summary.SteamId;
            _window = window;
            ContainingTab = containingTab;
            _chatHandler = chatHandler;

            Loaded += ChatControl_Loaded;

            InitializeComponent();
        }

        public void HandleMessage(string message)
        {
            AddMessage(Friend.Summary.PersonaName, message);
        }

        void ChatControl_Loaded(object sender, RoutedEventArgs e)
        {
            chatterFriendControl.SetFromFriendObject(Friend);
            messageBox.PreviewKeyUp += MessageBox_KeyPress;
        }

        void MessageBox_KeyPress(object sender, KeyEventArgs e)
        {
            e.Handled = true; //because dumb windows shit
            if (e.Key != Key.Enter) return;

            _chatHandler.Message(ChatterId, "saytext", messageBox.Text);
            AddMessage(FriendsListWindow.MySummary.PersonaName, messageBox.Text);
            messageBox.Text = string.Empty;
        }

        void AddMessage(string username, string message)
        {
            if (!chatBox.Dispatcher.CheckAccess())
            {
                chatBox.Dispatcher.Invoke(() => chatBox.Text += ("\n" + username + ": " + message),
                    DispatcherPriority.Normal);
            }
            else
            {
                chatBox.Text += ("\n" + username + ": " + message);
            }
        }

        void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            _chatHandler.Message(ChatterId, "saytext", messageBox.Text);
            AddMessage(FriendsListWindow.MySummary.PersonaName, messageBox.Text);
            messageBox.Text = string.Empty;
        }

        void closeTab_Click(object sender, RoutedEventArgs e)
        {
            _window.InvokeRemoveTab(ContainingTab);
        }
    }
}
