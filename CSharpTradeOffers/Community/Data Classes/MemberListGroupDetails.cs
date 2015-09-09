using System.Xml.Serialization;

namespace CSharpTradeOffers.Community
{
    /// <remarks/>
    [System.Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class MemberListGroupDetails
    {
        /// <remarks/>
        [XmlElement("groupName")]
        public string GroupName { get; set; }

        /// <remarks/>
        [XmlElement("groupURL")]
        public string GroupUrl { get; set; }

        /// <remarks/>
        [XmlElement("headline")]
        public string Headline { get; set; }

        /// <remarks/>
        [XmlElement("summary")]
        public string Summary { get; set; }

        /// <remarks/>
        [XmlElement("avatarIcon")]
        public string AvatarIcon { get; set; }

        /// <remarks/>
        [XmlElement("avatarMedium")]
        public string AvatarMedium { get; set; }

        /// <remarks/>
        [XmlElement("avatarFull")]
        public string AvatarFull { get; set; }

        /// <remarks/>
        [XmlElement("memberCount")]
        public ulong MemberCount { get; set; }

        /// <remarks/>
        [XmlElement("membersInChat")]
        public ulong MembersInChat { get; set; }

        /// <remarks/>
        [XmlElement("membersInGame")]
        public ulong MembersInGame { get; set; }

        /// <remarks/>
        [XmlElement("membersOnline")]
        public ulong MembersOnline { get; set; }
    }
}
