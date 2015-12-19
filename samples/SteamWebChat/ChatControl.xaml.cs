using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CSharpTradeOffers.Community;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl
    {
        private readonly SteamChatHandler _chatHandler;
        public readonly PlayerSummary Chatter;

        public ChatControl(SteamChatHandler chatHandler, PlayerSummary chatter)
        {
            _chatHandler = chatHandler;
            Chatter = chatter;

            Loaded += ChatControl_Loaded;

            InitializeComponent();
        }

        void ChatControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetAvatarUri(new Uri(Chatter.AvatarMedium));
            personaNameLabel.Content = Chatter.PersonaName;
            messageBox.PreviewKeyUp += MessageBox_KeyPress;
        }

        void MessageBox_KeyPress(object sender, KeyEventArgs e)
        {
            e.Handled = true; //because dumb windows shit
            if (e.Key != Key.Enter) return;

            _chatHandler.Message(Chatter.SteamId, "saytext", messageBox.Text);
            AddMessage(FriendsListWindow.MySummary.PersonaName, messageBox.Text);
            messageBox.Text = string.Empty;
        }

        public void HandleMessage(string message)
        {
            AddMessage(Chatter.PersonaName, message);
        }

        void SetAvatarUri(Uri uri)
        {
            if (!avatarImage.Dispatcher.CheckAccess())
            {
                avatarImage.Dispatcher.Invoke(() => avatarImage.Source = new BitmapImage(uri),
                    DispatcherPriority.Normal);
            }
            else
            {
                avatarImage.Source = new BitmapImage(uri);
            }
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

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            _chatHandler.Message(Chatter.SteamId, "saytext", messageBox.Text);
            AddMessage(FriendsListWindow.MySummary.PersonaName, messageBox.Text);
            messageBox.Text = string.Empty;
        }
    }
}
