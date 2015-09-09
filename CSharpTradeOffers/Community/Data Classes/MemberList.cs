using System.Xml.Serialization;

namespace CSharpTradeOffers.Community
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false,ElementName = "memberList")]
    public class MemberList
    {
        /// <remarks/>
        [XmlElement("groupID64")]
        public ulong GroupId64 { get; set; }

        /// <remarks/>
        [XmlElement("groupDetails")]
        public MemberListGroupDetails GroupDetails { get; set; }

        /// <remarks/>
        [XmlElement("memberCount")]
        public ulong memberCount { get; set; }

        /// <remarks/>
        [XmlElement("totalPages")]
        public ulong TotalPages { get; set; }

        /// <remarks/>
        [XmlElement("currentPage")]
        public ulong CurrentPage { get; set; }

        /// <remarks/>
        [XmlElement("startingMember")]
        public ulong StartingMember { get; set; }

        /// <remarks/>
        [XmlArrayItem("steamID64", IsNullable = false)]
        public ulong[] members { get; set; }
    }
}