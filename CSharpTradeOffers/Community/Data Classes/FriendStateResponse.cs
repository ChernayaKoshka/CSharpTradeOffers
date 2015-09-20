using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    [JsonObject(Title = "RootObject")]
    public class FriendStateResponse
    {
        [JsonProperty("m_unAccountID")]
        public int AccountID { get; set; }
        [JsonProperty("m_ulSteamID")]
        public string SteamID { get; set; }
        [JsonProperty("m_strName")]
        public string Name { get; set; }
        [JsonProperty("m_ePersonaState")]
        public int PersonaState { get; set; }
        [JsonProperty("m_nPersonaStateFlags")]
        public object PersonaStateFlags { get; set; }
        [JsonProperty("m_strAvatarHash")]
        public string AvatarHash { get; set; }
        [JsonProperty("m_tsLastMessage")]
        public int LastMessage { get; set; }
        [JsonProperty("m_tsLastView")]
        public int LastView { get; set; }
    }

}
