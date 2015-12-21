using CSharpTradeOffers.Community;

namespace SteamWebChat
{
    public class ChatUser
    {
        public PlayerSummary Summary { get; set; }

        public FriendStateResponse State { get; set; }
    }
}
