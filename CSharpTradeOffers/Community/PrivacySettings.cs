using System;

namespace CSharpTradeOffers.Community
{
#pragma warning disable 1591
    public class PrivacySettings

    {
        public const string type = "profileSettings";
        public EPrivacySetting privacySetting { get; set; }
        public EPrivacySetting commentSetting { get; set; } //commentselfonly,commentfriendsonly,commentanyone
        public EPrivacySetting InventoryPrivacySetting { get; set; }
        public bool inventoryGiftPrivacy { get; set; }
        public bool tradeConfirmationSetting { get; set; }
        public bool marketConfirmationSetting { get; set; }

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
#pragma warning restore 1591
