using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSharpTradeOffers.Community
{
    [XmlRoot(ElementName = "groupDetails")]
    public class GroupDetails
    {
        [XmlElement(ElementName = "groupName")]
        public string GroupName { get; set; }
        [XmlElement(ElementName = "groupURL")]
        public string GroupUrl { get; set; }
        [XmlElement(ElementName = "headline")]
        public string Headline { get; set; }
        [XmlElement(ElementName = "summary")]
        public string Summary { get; set; }
        [XmlElement(ElementName = "avatarIcon")]
        public string AvatarIcon { get; set; }
        [XmlElement(ElementName = "avatarMedium")]
        public string AvatarMedium { get; set; }
        [XmlElement(ElementName = "avatarFull")]
        public string AvatarFull { get; set; }
        [XmlElement(ElementName = "memberCount")]
        public int MemberCount { get; set; }
        [XmlElement(ElementName = "membersInChat")]
        public int MembersInChat { get; set; }
        [XmlElement(ElementName = "membersInGame")]
        public int MembersInGame { get; set; }
        [XmlElement(ElementName = "membersOnline")]
        public int MembersOnline { get; set; }
    }

    [XmlRoot(ElementName = "members")]
    public class Members
    {
        [XmlElement(ElementName = "steamID64")]
        public List<ulong> MemberSteamIds { get; set; }
    }

    [XmlRoot(ElementName = "memberList")]
    public class MemberList
    {
        [XmlElement(ElementName = "groupID64")]
        public ulong GroupId64 { get; set; }
        [XmlElement(ElementName = "groupDetails")]
        public GroupDetails GroupDetails { get; set; }
        [XmlElement(ElementName = "memberCount")]
        public int MemberCount { get; set; }
        [XmlElement(ElementName = "totalPages")]
        public int TotalPages { get; set; }
        [XmlElement(ElementName = "currentPage")]
        public int CurrentPage { get; set; }
        [XmlElement(ElementName = "startingMember")]
        public ulong StartingMember { get; set; }
        [XmlElement(ElementName = "members")]
        public Members Members { get; set; }
    }
}