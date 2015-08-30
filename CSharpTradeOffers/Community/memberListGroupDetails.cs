using System.Xml.Serialization;

namespace CSharpTradeOffers.Community
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class MemberListGroupDetails
    {
        /// <remarks/>
        public string groupName { get; set; }

        /// <remarks/>
        public string groupURL { get; set; }

        /// <remarks/>
        public string headline { get; set; }

        /// <remarks/>
        public string summary { get; set; }

        /// <remarks/>
        public string avatarIcon { get; set; }

        /// <remarks/>
        public string avatarMedium { get; set; }

        /// <remarks/>
        public string avatarFull { get; set; }

        /// <remarks/>
        public ulong memberCount { get; set; }

        /// <remarks/>
        public ulong membersInChat { get; set; }

        /// <remarks/>
        public ulong membersInGame { get; set; }

        /// <remarks/>
        public ulong membersOnline { get; set; }
    }
}