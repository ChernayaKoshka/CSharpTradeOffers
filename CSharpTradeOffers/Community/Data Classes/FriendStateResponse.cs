using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class FriendStateResponse
    {
        public int m_unAccountID { get; set; }
        public string m_ulSteamID { get; set; }
        public string m_strName { get; set; }
        public int m_ePersonaState { get; set; }
        public object m_nPersonaStateFlags { get; set; }
        public string m_strAvatarHash { get; set; }
        public int m_tsLastMessage { get; set; }
        public int m_tsLastView { get; set; }
    }

}
