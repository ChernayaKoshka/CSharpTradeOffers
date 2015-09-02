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
        public ulong groupID64 { get; set; }

        /// <remarks/>
        public MemberListGroupDetails groupDetails { get; set; }

        /// <remarks/>
        public ulong memberCount { get; set; }

        /// <remarks/>
        public ulong totalPages { get; set; }

        /// <remarks/>
        public ulong currentPage { get; set; }

        /// <remarks/>
        public ulong startingMember { get; set; }

        /// <remarks/>
        [XmlArrayItem("steamID64", IsNullable = false)]
        public ulong[] members { get; set; }
    }
}