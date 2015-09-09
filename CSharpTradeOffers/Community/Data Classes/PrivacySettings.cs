using System;

namespace CSharpTradeOffers.Community
{
    using Newtonsoft.Json;

    public class PrivacySettings

    {
        [JsonProperty("type")]
        public const string Type = "profileSettings";
        [JsonProperty("privacySetting")]
        public EPrivacySetting PrivacySetting { get; set; }
        [JsonProperty("commentSetting")]
        public EPrivacySetting CommentSetting { get; set; } //commentselfonly,commentfriendsonly,commentanyone
        [JsonProperty("InventoryPrivacySetting")]
        public EPrivacySetting InventoryPrivacySetting { get; set; }
        [JsonProperty("inventoryGiftPrivacy")]
        public bool InventoryGiftPrivacy { get; set; }
        [JsonProperty("tradeConfirmationSetting")]
        public bool TradeConfirmationSetting { get; set; }
        [JsonProperty("marketConfirmationSetting")]
        public bool MarketConfirmationSetting { get; set; }

        public static string EPrivacySettingToCommentSetting(EPrivacySetting setting)
        {
            switch (setting)
            {
                case EPrivacySetting.Private:
                    return "commentselfonly";
                case EPrivacySetting.FriendsOnly:
                    return "commentfriendsonly";
                case EPrivacySetting.Public:
                    return "commentanyone";
                default:
                    throw new Exception("By some miracle the PrivacySetting wasn't covered by the switch!");
            }
        }
    }

    public enum EPrivacySetting
    {
        Private = 1,
        FriendsOnly = 2,
        Public = 3
    }
}
