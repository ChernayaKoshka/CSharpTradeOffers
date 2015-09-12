using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Community
{
    /// <summary>
    /// Handles commmunity related tasks
    /// </summary>
    public class CommunityHandler
    {
        private readonly Web _web = new Web(new SteamRequestHandler());

        /// <summary>
        /// Posts a comment to the specified profile.
        /// </summary>
        /// <param name="steamId">The SteamId64 of the profile to post to.</param>
        /// <param name="comment">The comment text.</param>
        /// <param name="authContainer">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <returns>A CommentResponse object.</returns>
        public CommentResponse PostComment(ulong steamId, string comment, CookieContainer authContainer)
        {
            string url = "https://steamcommunity.com/comment/Profile/post/" + steamId + "/-1/";

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                where cookie.Name == "sessionid"
                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"comment", comment},
                {"sessionid", sessionid}
            };

            return
                JsonConvert.DeserializeObject<CommentResponse>(_web.Fetch(url, "POST", data, authContainer, true,
                    url.Substring(0, url.Length - 3)));
        }

        /// <summary>
        /// Posts a comment to the specified clan.
        /// </summary>
        /// <param name="clanId">SteamID64 of the clan to post the comment to.</param>
        /// <param name="comment">The comment to post.</param>
        /// <param name="authContainer">Authcontainer of the user to post from.</param>
        /// <param name="count">Almost certainly useless and never needs to be touched.
        /// I assume that it is the member count but it can be null, non-existant, or any number under the sun.</param>
        /// <returns>A ClanCommentResponse object.</returns>
        public ClanCommentResponse PostClanComment(ulong clanId, string comment, CookieContainer authContainer,
            int count = 0)
        {
            string url = "http://steamcommunity.com/comment/Clan/post/" + clanId + "/-1/";

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                where cookie.Name == "sessionid"
                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"comment", comment},
                {"count", count.ToString()},
                {"sessionid", sessionid}
            };

            return JsonConvert.DeserializeObject<ClanCommentResponse>(_web.Fetch(url, "POST", data, authContainer));
        }

        /// <summary>
        /// Sends a friend request to the specified user.
        /// </summary>
        /// <param name="steamId">SteamId64 of the user to add.</param>
        /// <param name="authContainer">Auth container of the user.</param>
        /// <returns>An AddFriendResponse object.</returns>
        public AddFriendResponse AddFriend(ulong steamId, CookieContainer authContainer)
        {
            string url = "https://steamcommunity.com/actions/AddFriendAjax";

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionID", sessionid},
                {"steamid", steamId.ToString()},
                {"accept_invite", "0"}
            };

            return JsonConvert.DeserializeObject<AddFriendResponse>(_web.Fetch(url, "POST", data, authContainer));
        }

        /// <summary>
        /// Accepts a friend request.
        /// </summary>
        /// <param name="steamId">SteamId64 of the person who sent the request.</param>
        /// <param name="authContainer">Auth container of the user.</param>
        /// <returns>Boolean representing the success of the function.</returns>
        public bool AcceptFriendRequest(ulong steamId, CookieContainer authContainer)
        {
            string url = "https://steamcommunity.com/actions/AddFriendAjax";

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionID", sessionid},
                {"steamid", steamId.ToString()},
                {"accept_invite","1" }
            };

            return Convert.ToBoolean(_web.Fetch(url, "POST", data, authContainer));
        }

        /// <summary>
        /// Removes a friend.
        /// </summary>
        /// <param name="steamId">SteamId64 of the friend to remove.</param>
        /// <param name="authContainer">Auth container of the user.</param>
        /// <returns>Bollean representing the successs of the function.</returns>
        public bool RemoveFriend(ulong steamId, CookieContainer authContainer)
        {
            string url = "https://steamcommunity.com/actions/RemoveFriendAjax";

            string sessionid = (from Cookie cookie in authContainer.GetCookies(new Uri("https://steamcommunity.com"))
                                where cookie.Name == "sessionid"
                                select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionID", sessionid},
                {"steamid", steamId.ToString()},
            };

            return Convert.ToBoolean(_web.Fetch(url, "POST", data, authContainer));
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
                {"invitee", steamId.ToString()}
            };
            return JsonConvert.DeserializeObject<InviteResponse>(_web.Fetch(url, "POST", data, authContainer));
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
            return JsonConvert.DeserializeObject<MultiInviteResponse>(_web.Fetch(url, "POST", data, authContainer));
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
                    new XmlSerializer(typeof (MemberList)).Deserialize(_web.FetchStream(string.Format(url, groupId),
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
                    new XmlSerializer(typeof (MemberList)).Deserialize(_web.FetchStream(string.Format(url, groupName),
                        "GET"));
        }

        /// <summary>
        /// Requests group information as well as all pages of users.
        /// </summary>
        /// <param name="groupId">The SteamId64 of the group to request information about.</param>
        /// <param name="retryWait">The number of miliseconds to wait between each retry.</param>
        /// <param name="retryCount">The number of times to retry before inserting a null MemberList object.</param>
        /// <returns>A List of the MemberList object.</returns>
        public List<MemberList> RequestAllMemberLists(ulong groupId, int retryWait = 1000, int retryCount = 10)
        {
            var membersList = new List<MemberList>();
            const string url = "http://steamcommunity.com/gid/{0}/memberslistxml/?xml=1&p={1}";

            ulong count = 1;
            ulong totalPages = 1;
            bool firstRequest = false;

            do
            {
                string temp = string.Format(url, groupId, count);

                try
                {
                    var populatedList = (MemberList)
                        (new XmlSerializer(typeof (MemberList)).Deserialize(_web.RetryFetchStream(retryWait, retryCount,
                            temp, "GET")));
                    membersList.Add(populatedList);
                    if (!firstRequest)
                    {
                        firstRequest = true;
                        totalPages = populatedList.TotalPages;
                    }
                }
                catch (WebException)
                {
                    membersList.Add(null);
                }
                catch (NullReferenceException)
                {
                    membersList.Add(null);
                }
                Thread.Sleep(1000);

                count++;
            } while (totalPages >= count);

            return membersList;
        }

        /// <summary>
        /// Requests group information as well as the all pages of users.
        /// </summary>
        /// <param name="groupName">The name of the group to request information about.</param>
        /// <param name="retryWait">The number of miliseconds to wait between each retry.</param>
        /// <param name="retryCount">The number of times to retry before inserting a null MemberList object.</param>
        /// <returns>A List of the MemberList object.</returns>
        public List<MemberList> RequestAllMemberLists(string groupName, int retryWait = 1000, int retryCount = 10)
        {
            var membersList = new List<MemberList>();
            groupName = groupName.Replace(" ", "");
            const string url = "http://steamcommunity.com/groups/{0}/memberslistxml/?xml=1&p={1}";

            ulong count = 1;
            ulong totalPages = 1;
            bool firstRequest = false;
            do
            {
                string temp = string.Format(url, groupName, count);

                try
                {
                    var populatedList = (MemberList)
                        (new XmlSerializer(typeof (MemberList)).Deserialize(_web.RetryFetchStream(retryWait, retryCount,
                            temp, "GET")));
                    membersList.Add(populatedList);

                    if (!firstRequest)
                    {
                        firstRequest = true;
                        totalPages = populatedList.TotalPages;
                    }
                }
                catch (WebException)
                {
                    membersList.Add(null);
                }
                catch (NullReferenceException)
                {
                    membersList.Add(null);
                }
                Thread.Sleep(1000);

                count++;
            } while (totalPages >= count);

            return membersList;
        }

        /// <summary>
        /// Sets the user's public profile settings to the exact settings specified in
        /// the Profile object.
        /// </summary>
        /// <param name="profile">The object to specify the new profile data.</param>
        /// <param name="account">The account of the profile to modify.</param>
        /// <returns>Bool depending on the success of the request.</returns>
        public bool SetProfile(Profile profile, Account account) //implement Settings as an interface!
        {
            string url = "https://steamcommunity.com/profiles/" + account.SteamId + "/edit";

            string sessionid =
                (from Cookie cookie in account.AuthContainer.GetCookies(new Uri("https://steamcommunity.com"))
                    where cookie.Name == "sessionid"
                    select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionID", sessionid},
                {"type", Profile.Type},
                {"weblink_1_title", profile.Weblink1Title},
                {"weblink_1_url", profile.Weblink1Url},
                {"weblink_2_title", profile.Weblink2Title},
                {"weblink_2_url", profile.Weblink2Url},
                {"weblink_3_title", profile.Weblink3Title},
                {"weblink_3_url", profile.Weblink3Url},
                {"personaName", profile.PersonaName},
                {"real_name", profile.RealName},
                {"country", profile.Country},
                {"state", profile.State},
                {"city", profile.City},
                {"customURL", profile.CustomUrl},
                {"summary", profile.Summary},
                {"favorite_badge_badgeid", profile.FavoriteBadgeBadgeId.ToString()},
                {"favorite_badge_communityitemid", profile.FavoriteBadgeCommunityItemId.ToString()},
                {"primary_group_steamid", profile.PrimaryGroupSteamId.ToString()}
            };

            string response = _web.Fetch(url, "POST", data, account.AuthContainer);
            return response.Contains("<div class=\"saved_changes_msg\">");
        }

        /// <summary>
        /// Sets the privacy settings of the account.
        /// </summary>
        /// <param name="settings">Settings to set.</param>
        /// <param name="account">Account of settings to change.</param>
        public void SetPrivacySettings(PrivacySettings settings, Account account)
            //implement settings as an interface later!
        {
            string url = "https://steamcommunity.com/profiles/" + account.SteamId + "/edit/settings";

            string sessionid =
                (from Cookie cookie in account.AuthContainer.GetCookies(new Uri("https://steamcommunity.com"))
                    where cookie.Name == "sessionid"
                    select cookie.Value).FirstOrDefault();

            var data = new Dictionary<string, string>
            {
                {"sessionID", sessionid},
                {"type", PrivacySettings.Type},
                {"privacySetting", ((int) settings.PrivacySetting).ToString()},
                {"commentSetting", PrivacySettings.EPrivacySettingToCommentSetting(settings.CommentSetting)},
                {"inventoryPrivacySetting", ((int) settings.InventoryPrivacySetting).ToString()},
                {"inventoryGiftPrivacy", settings.InventoryGiftPrivacy.IntValue().ToString()},
                {"tradeConfirmationSetting", settings.TradeConfirmationSetting.IntValue().ToString()},
                {"marketConfirmationSetting", settings.MarketConfirmationSetting.IntValue().ToString()}
            };
            _web.Fetch(url, "POST", data, account.AuthContainer);
        }
    }
}
