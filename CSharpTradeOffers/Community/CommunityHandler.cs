using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// Handles commmunity related tasks
    /// </summary>
    public class CommunityHandler
    {
        /// <summary>
        /// Posts a comment to the specified profile.
        /// </summary>
        /// <param name="steamId">The SteamId64 of the profile to post to.</param>
        /// <param name="comment">The comment text.</param>
        /// <param name="authContainer">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A CommentResponse object.</returns>
        public CommentResponse PostComment(ulong steamId, string comment, CookieContainer authContainer)
        {
            string url = "https://steamcommunity.com/comment/Profile/post/{0}/-1/";
            url = string.Format(url, steamId);

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                where cookie.Name == "sessionid"
                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"comment", comment},
                {"sessionid", sessionid}
            };

            return
                JsonConvert.DeserializeObject<CommentResponse>(Web.Fetch(url, "POST", data, authContainer, true,
                    url.Substring(0, url.Length - 3)));
        }

        /// <summary>
        /// Invites the specified user to a group.
        /// </summary>
        /// <param name="steamId">The SteamId64 of the person to invite.</param>
        /// <param name="json">I forgot... oops.</param>
        /// <param name="group">The SteamId64 of the group to invite to.</param>
        /// <param name="authContainer">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>An InviteResponse object.</returns>
        public InviteResponse InviteUserToGroup(ulong steamId, bool json, ulong group,
            CookieContainer authContainer)
        {
            const string url = "https://steamcommunity.com/actions/GroupInvite/";
            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                where cookie.Name == "sessionid"
                select cookie.Value).FirstOrDefault();
            var data = new Dictionary<string, string>
            {
                {"json", json.IntValue().ToString()},
                {"type", "groupInvite"},
                {"group", group.ToString()},
                {"sessionID", sessionid},
                {"invitee_list", ToJArray(new[] {steamId})}
            };
            return JsonConvert.DeserializeObject<InviteResponse>(Web.Fetch(url, "POST", data, authContainer));
        }

        /// <summary>
        /// Invites the specified users to a group.
        /// </summary>
        /// <param name="steamIds">The SteamId64s of the users to invite.</param>
        /// <param name="json">I forgot... oops.</param>
        /// <param name="group">The SteamId64 of the group to invite the users to.</param>
        /// <param name="authContainer">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A MultiInviteResponse object.</returns>
        public MultiInviteResponse InviteUsersToGroup(ulong[] steamIds, bool json, ulong group,
            CookieContainer authContainer)
        {
            const string url = "https://steamcommunity.com/actions/GroupInvite/";
            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                where cookie.Name == "sessionid"
                select cookie.Value).FirstOrDefault();
            var data = new Dictionary<string, string>
            {
                {"json", json.IntValue().ToString()},
                {"type", "groupInvite"},
                {"group", group.ToString()},
                {"sessionID", sessionid},
                {"invitee_list", ToJArray(steamIds)}
            };
            return JsonConvert.DeserializeObject<MultiInviteResponse>(Web.Fetch(url, "POST", data, authContainer));
        }
        
        /// <summary>
        /// Converts an array of SteamId64s to an array of the format ["0","1"]
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public string ToJArray(ulong[] items)
        {
            const string formatter = "\"{0}\",";
            string jArray = items.Aggregate("[", (current, item) => current + string.Format(formatter, item.ToString()));
            jArray = jArray.Substring(0, jArray.Length - 1);
            jArray += "]";
            return jArray;
        }

        /// <summary>
        /// Requests group information as well as the first page of users.
        /// </summary>
        /// <param name="groupId">The SteamId64 of the group to request information about.</param>
        /// <returns>A memberList object.</returns>
        public MemberList RequestMemberList(ulong groupId)
        {
            const string url = "http://steamcommunity.com/gid/{0}/memberslistxml/?xml=1";

            return
                (MemberList)
                    new XmlSerializer(typeof (MemberList)).Deserialize(Web.FetchStream(string.Format(url, groupId),
                        "GET"));
        }

        /// <summary>
        /// Requests group information as well as the first page of users.
        /// </summary>
        /// <param name="groupName">The name of the group to request information about.</param>
        /// <returns>A memberList object.</returns>
        public MemberList RequestMemberList(string groupName)
        {
            const string url = "http://steamcommunity.com/groups/{0}/memberslistxml/?xml=1";

            return
                (MemberList)
                    new XmlSerializer(typeof (MemberList)).Deserialize(Web.FetchStream(string.Format(url, groupName),
                        "GET"));
        }

        /// <summary>
        /// Requests group information as well as all pages of users.
        /// </summary>
        /// <param name="groupId">The SteamId64 of the group to request information about.</param>
        /// <returns>A List of the memberList object.</returns>
        public List<MemberList> RequestAllMemberLists(ulong groupId)
        {
            var membersList = new List<MemberList>();
            const string url = "http://steamcommunity.com/gid/{0}/memberslistxml/?xml=1&p={1}";

            MemberList populatedList;
            ulong count = 1;

            do
            {
                string temp = string.Format(url, groupId, count);

                populatedList =
                    (MemberList) (new XmlSerializer(typeof (MemberList)).Deserialize(Web.FetchStream(temp, "GET")));
                membersList.Add(populatedList);

                count++;
            } while (populatedList.totalPages >= count);

            return membersList;
        }

        /// <summary>
        /// Requests group information as well as the all pages of users.
        /// </summary>
        /// <param name="groupName">The name of the group to request information about.</param>
        /// <returns>A List of the memberList object.</returns>
        public List<MemberList> RequestAllMemberLists(string groupName)
        {
            var membersList = new List<MemberList>();
            groupName = groupName.Replace(" ", "");
            const string url = "http://steamcommunity.com/groups/{0}/memberslistxml/?xml=1&p={1}";

            MemberList populatedList;
            ulong count = 1;

            do
            {
                string temp = string.Format(url, groupName, count);

                populatedList =
                    (MemberList) (new XmlSerializer(typeof (MemberList)).Deserialize(Web.FetchStream(temp, "GET")));
                membersList.Add(populatedList);

                count++;
            } while (populatedList.totalPages >= count);

            return membersList;
        }
    }
}
