using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CSharpTradeOffers.Community;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for FriendControl.xaml
    /// </summary>
    public partial class FriendControl : UserControl
    {
        public ChatUser Friend { get; private set; }

        [Obsolete]
        public FriendControl()
        {
            InitializeComponent();
        }

        public FriendControl(ChatUser friend)
        {
            InitializeComponent();

            SetFromFriendObject(friend);
        }

        public void SetFromFriendObject(ChatUser friend)
        {
            Friend = friend;

            SolidColorBrush stateBrush = StateToBrush(friend.State.PersonaState, friend.State.InGame);

            personaName.Text = friend.Summary.PersonaName;
            personaName.Foreground = stateBrush;

            personaState.Text = StateToText(friend.State.PersonaState) + (friend.State.InGame ? " and in-game" : "");
            personaState.Foreground = stateBrush;

            avatarImage.Source = new BitmapImage(new Uri(friend.Summary.AvatarMedium));

            avatarBorder.BorderBrush = stateBrush;

            inGameName.Text = friend.State.InGameName;
            inGameName.Foreground = stateBrush;
        }

        static string StateToText(EPersonaState state)
        {
            switch (state)
            {
                case EPersonaState.Offline:
                    return "Offline";
                case EPersonaState.Online:
                    return "Online";
                case EPersonaState.Away:
                    return "Away";
                case EPersonaState.Busy:
                    return "Busy";
                case EPersonaState.Snooze:
                    return "Snooze";
            }
            return "Unknown state";
        }

        static SolidColorBrush StateToBrush(EPersonaState state, bool inGame)
        {
            switch (state)
            {
                case EPersonaState.Offline:
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF636363"));
                default:
                    if (inGame)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7DBA35"));
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF509BB8"));
            }
        }
    }
}
