using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CSharpTradeOffers.Configuration;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Trading
{
    public class EconServiceHandler
    {
        //create initliazer to pass inventoryhandler?
        private static readonly ISteamEconomyHandler EconomyHandler = new ISteamEconomyHandler();
        private static readonly MarketHandler MarketHandler = new MarketHandler();
        private readonly string _apiKey ;

        private const string ApiUrl = "https://api.steampowered.com/IEconService/{0}/v1/"; //migrate to base url later
        private const string BaseUrl = "https://api.steampowered.com/IEconService/";

        public EconServiceHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Requests all trade offers.
        /// </summary>
        /// <param name="_apiKey">The bot's API key.</param>
        /// <param name="data">A dictionary containing the URL params found here: https://developer.valvesoftware.com/wiki/Steam_Web_API/IEconService at GetTradeOffers (v1).
        /// ex: <"get_sent_offers","1"></param>
        /// <returns></returns>
        public TradeOffersList GetTradeOffers(Dictionary<string, string> data)
        {
            data.Add("key", _apiKey);
            data.Add("format", "json");
            return
                JsonConvert.DeserializeObject<TradeOffers>(
                    WebUtility.UrlDecode(Web.Fetch(string.Format(ApiUrl, "GetTradeOffers"), "GET", data))).response;
        }

        /// <summary>
        /// Requests a single trade offer based on the tradeofferid.
        /// </summary>
        /// <param name="_apiKey">The bot's API key.</param>
        /// <param name="tradeofferid">The tradeofferid to request information on.</param>
        /// <param name="language">The language to use. Default: english</param>
        /// <returns>A CEConTradeOffer object.</returns>
        public CEconTradeOffer GetTradeOffer(string tradeofferid, string language = "english")
        {

            if (_apiKey == null) throw new ArgumentNullException(nameof(_apiKey));
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"language", language},
                {"format", "json"}
            };
            return
                JsonConvert.DeserializeObject<CEconTradeOffer>(
                    WebUtility.UrlDecode(Web.Fetch(string.Format(ApiUrl, "GetTradeOffer"), "GET", data)));
        }

        /// <summary>
        /// Declines a trade offer that was sent to you.
        /// </summary>
        /// <param name="_apiKey">The bot's API key.</param>
        /// <param name="tradeofferid">The ID of the offer to decline.</param>
        /// <returns></returns>
        public string DeclineTradeOffer(string tradeofferid)
        {
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"format", "json"}
            };
            return Web.Fetch(string.Format(ApiUrl, "DeclineTradeOffer"), "POST", data);
        }

        /// <summary>
        /// Cancels a trade offer based on the specified tradeofferid.
        /// </summary>
        /// <param name="_apiKey">The bot's API Key.</param>
        /// <param name="tradeofferid">The ID of the offer to cancel.</param>
        /// <returns></returns>
        public string CancelTradeOffer(string tradeofferid)
        {
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"tradeofferid", tradeofferid},
                {"format", "json"}
            };
            return WebUtility.UrlDecode(Web.Fetch(string.Format(ApiUrl, "CancelTradeOffer"), "POST", data));
        }

        /// <summary>
        /// Accepts a specified trade offer.
        /// </summary>
        /// <param name="tradeId">A TradeId object containing the id of the trade to accept.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="partnerId">The AccountId of the person to trade with.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <returns>The TradeId of the offer that was accepted.</returns>
        public TradeId AcceptTradeOffer(TradeId tradeId, CookieContainer container, uint partnerId, string serverid)
        {
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });
            const string url = "https://steamcommunity.com/tradeoffer/{0}/accept";
            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"tradeofferid", tradeId.tradeid.ToString()},
                {"partner", SteamIdOperations.ConvertAccountIdtoUInt64(partnerId).ToString()},
                {"captcha", ""}
            };
            return
                JsonConvert.DeserializeObject<TradeId>(
                    WebUtility.UrlDecode(Web.Fetch(string.Format(url, tradeId.tradeid), "POST",
                        data, container, false, "https://steamcommunity.com/tradeoffer/" + tradeId.tradeid + "/")));
        }

        /// <summary>
        /// Sends a trade offer to the specified recipient. 
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person to send the offer to.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="tradeoffermessage">An optional message to be sent with the offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters.</param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse SendTradeOffer(ulong partnerSid, CookieContainer container, string tradeoffermessage,
            string serverid, TradeOffer offer)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });

            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", ""},
                {"trade_offer_create_params", "{}"}
            };
            return JsonConvert.DeserializeObject<SendOfferResponse>(Web.Fetch(url, "POST", data, container, false,
                "https://steamcommunity.com/tradeoffer/new/?partner=" +
                SteamIdOperations.ConvertSteamIdToAccountId(SteamIdOperations.ConvertUlongToSteamId(partnerSid))));
        }

        /// <summary>
        /// Modifies an existing trade offer.
        /// </summary>
        /// <param name="partnerSid">The SteamId64 (ulong) of the person whose offer will be modified.</param>
        /// <param name="container">Auth Cookies MUST be passed here, the function will fail if not.</param>
        /// <param name="tradeoffermessage">An otpional message to be sent with the trade offer. Can be null.</param>
        /// <param name="serverid">Almost always 1, not quite sure what other numbers do.</param>
        /// <param name="tradeofferidCountered">The TradeId of the offer to counter or modify.</param>
        /// <param name="offer">A TradeOffer object containing the trade parameters. </param>
        /// <returns>A SendOfferResponse object.</returns>
        public SendOfferResponse ModifyTradeOffer(ulong partnerSid, CookieContainer container, string tradeoffermessage,
            string serverid, uint tradeofferidCountered, TradeOffer offer)
        {
            const string url = "https://steamcommunity.com/tradeoffer/new/send";
            container.Add(new Cookie("bCompletedTradeOfferTutorial", "true") { Domain = "steamcommunity.com" });

            var data = new Dictionary<string, string>
            {
                {"sessionid", Web.SessionId},
                {"serverid", serverid},
                {"partner", partnerSid.ToString()},
                {"tradeoffermessage", tradeoffermessage},
                {"json_tradeoffer", JsonConvert.SerializeObject(offer)},
                {"captcha", ""},
                {"trade_offer_create_params", "{}"},
                {"tradeofferid_countered", tradeofferidCountered.ToString()}
            };
            return JsonConvert.DeserializeObject<SendOfferResponse>(Web.Fetch(url, "POST", data, container, false,
                "https://steamcommunity.com/tradeoffer/" + tradeofferidCountered + "/"));
        }

        /// <summary>
        /// Attempts to locate a valid trade in the TradeConfig.
        /// </summary>
        /// <param name="_apiKey">The bot's API Key.</param>
        /// <param name="offer">A TradeOffer object containing the trade to create a compatible form with.</param>
        /// <param name="inventoryHandler">An InventoryHandler object, the InventoryHandler must be that off the bot otherwise it will return erroneous data.</param>
        /// <returns>A compatible trade offer, null if none was found.</returns>
        public TradeOffer CreateCompatibleTrade(CEconTradeOffer offer, InventoryHandler inventoryHandler)
        {
            TradeOffer compatibleOffer = new TradeOffer { them = { assets = offer.items_to_receive } };
            TradeConfig.AcceptableTrade acceptableTrade = FindAcceptableTrade(offer);
            if (acceptableTrade == null) return null;
            
            foreach (TradeConfig.ConfigAsset me in acceptableTrade.Me)
            {
                for (int i = 0; i < me.Amount; i++)
                {
                    Item item = inventoryHandler.FindUnusedItem(me);
                    rgInventory_Item rgIItem =
                        inventoryHandler.Inventories[Convert.ToUInt32(item.appid)].FindAvailableAsset(item.classid);
                    rgIItem.inUse = true;
                    CEconAsset assetToAdd = rgIItem.ToCEconAsset(item.appid);

                    compatibleOffer.me.assets.Add(assetToAdd);
                }
            }
            //return compatibleOffer.me.assets.Count == 0 ? null : compatibleOffer;
            return compatibleOffer;
        }

        /// <summary>
        /// Searches TradeConfig for a valid trade.
        /// </summary>
        /// <param name="offer">a CEconTradeOffer to check.</param>
        /// <returns>A TradeConfig.AcceptableTrade object.</returns>
        public TradeConfig.AcceptableTrade FindAcceptableTrade(CEconTradeOffer offer)
        {
            bool assetsAreAcceptable = false;
            var items_to_receive = offer.items_to_receive;
            GetNames(ref items_to_receive);
            foreach (TradeConfig.AcceptableTrade acceptableTrade in TradeConfig.TradesConfig.AcceptableTrades)
            {
                foreach (TradeConfig.ConfigAsset expectedAsset in acceptableTrade.Them)
                {
                    foreach (CEconAsset cEconAsset in items_to_receive)
                    {
                        
                        switch (expectedAsset.TypeId)
                        {
                            //TODO: CONTINUE HERE.
                            case 0:
                                if (cEconAsset.name == expectedAsset.TypeObj)
                                    if (expectedAsset.Amount == items_to_receive.Count(x => string.Equals(x.name, expectedAsset.TypeObj, StringComparison.CurrentCultureIgnoreCase)))
                                        assetsAreAcceptable = true;
                                break;
                            case 1:
                                if (expectedAsset.Amount ==
                                    items_to_receive.Count(
                                        x => x.name.Contains(expectedAsset.TypeObj.ToLower())))
                                    assetsAreAcceptable = true;
                                break;
                            case 2:
                                if (expectedAsset.Amount ==
                                    items_to_receive.Count(
                                        x => x.name.StartsWith(expectedAsset.TypeObj.ToLower())))
                                    assetsAreAcceptable = true;
                                break;
                            case 3:
                                if (expectedAsset.Amount == items_to_receive.Count(x => x.classid == expectedAsset.TypeObj))
                                    assetsAreAcceptable = true;
                                break;
                            case 4: //TODO: LEFT HERE v Need to work on this fucking shit
                                if (
                                    Convert.ToDecimal(
                                        MarketHandler.GetPriceOverview(Convert.ToUInt32(cEconAsset.appid),
                                            cEconAsset.GetMarketHashName(_apiKey)).median_price) >=
                                    Convert.ToDecimal(expectedAsset.TypeObj))
                                    assetsAreAcceptable = true;
                                break;
                            case 5:
                                var classid = new Dictionary<string, string>
                                {
                                    {cEconAsset.classid, cEconAsset.instanceid}
                                };
                                AssetClassInfo info = EconomyHandler.ToAssetClassInfo(
                                    EconomyHandler.GetAssetClassInfo(_apiKey, Convert.ToUInt32(cEconAsset.appid),
                                        classid).result);
                                foreach (Tag tag in info.tags.Where(tag => tag.name == expectedAsset.TypeObj))
                                    assetsAreAcceptable = true;
                                break;
                            case 6:
                                throw new Exception("'Them' is not a valid area for this TypeId!");
                            default:
                                throw new Exception("Unknown TypeId!");
                        }
                        break;
                    }
                    if (!assetsAreAcceptable) break;
                }
                if (assetsAreAcceptable) return acceptableTrade;
            }
            return null;
        }

        public void GetNames(ref List<CEconAsset> assets)
        {
            foreach (CEconAsset cEconAsset in assets)
                cEconAsset.name = cEconAsset.GetMarketHashName(_apiKey);
        } 

        #region old
        /*public TradeConfig.AcceptableTrade FindAcceptableTrade(CEconTradeOffer offer)
        {
            bool assetIsAcceptable = false;
            foreach (TradeConfig.AcceptableTrade acceptableTrade in TradeConfig.TradesConfig.AcceptableTrades)
            {
                foreach (CEconAsset cEconAsset in offer.items_to_receive)
                {
                    foreach (TradeConfig.ConfigAsset expectedAsset in acceptableTrade.Them)
                    {
                        switch (expectedAsset.TypeId)
                        {
                            case 0:
                                if (cEconAsset.GetMarketHashName(_apiKey) == expectedAsset.TypeObj)
                                    assetIsAcceptable = true;
                                break;
                            case 1:
                                if (
                                    cEconAsset.GetMarketHashName(_apiKey)
                                        .ToLower()
                                        .Contains(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 2:
                                if (
                                    cEconAsset.GetMarketHashName(_apiKey)
                                        .ToLower()
                                        .StartsWith(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 3:
                                if (cEconAsset.classid == expectedAsset.TypeObj)
                                    assetIsAcceptable = true;
                                break;
                            case 4:
                                if (
                                    Convert.ToUInt32(
                                        MarketHandler.GetPriceOverview(Convert.ToUInt32(cEconAsset.appid),
                                            cEconAsset.GetMarketHashName(_apiKey)).median_price) >=
                                    Convert.ToUInt32(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 5:
                                var classid = new Dictionary<string, string>
                                {
                                    {cEconAsset.classid, cEconAsset.instanceid}
                                };
                                AssetClassInfo info = EconomyHandler.ToAssetClassInfo(
                                    EconomyHandler.GetAssetClassInfo(_apiKey, Convert.ToUInt32(cEconAsset.appid),
                                        classid).result);
                                foreach (Tag tag in info.tags.Where(tag => tag.name == expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 6:
                                throw new Exception("'Them' is not a valid area for this TypeId!");
                            default:
                                throw new Exception("Unknown TypeId!");
                        }
                        break;
                    }
                    if (!assetIsAcceptable) break;
                }
                if (assetIsAcceptable) return acceptableTrade;
            }
            return null;
        }*/
        #endregion

        /// <summary>
        /// Requestss TradeOffer statistics. ie: historical_received_count
        /// </summary>
        /// <param name="time_last_visit">Unix time for historical cutoff.</param>
        /// <returns></returns>
        public GetTradeOffersSummaryResponse GetTradeOffersSummary(uint time_last_visit)
        {
            const string url = BaseUrl + "GetTradeOffersSummary/v1/";
            var data = new Dictionary<string, string>
            {
                {"key", _apiKey},
                {"time_last_visit", time_last_visit.ToString()}
            };
            return
                JsonConvert.DeserializeObject<GetTradeOffersSummaryBaseResponse>(Web.Fetch(url, "GET", data)).response;
        }

        /*Non LINQ
        public TradeConfig.AcceptableTrade FindAcceptableTrade(string apiKey, CEconTradeOffer offer)
        {
            bool assetIsAcceptable = false;
            foreach (TradeConfig.AcceptableTrade acceptableTrade in TradeConfig.TradesConfig.AcceptableTrades)
            {
                foreach (CEconAsset cEconAsset in offer.items_to_receive)
                {
                    foreach (TradeConfig.ConfigAsset expectedAsset in acceptableTrade.Them)
                    {
                        switch (expectedAsset.TypeId)
                        {
                            case 0:
                                if (cEconAsset.GetMarketHashName(Config.Cfg.ApiKey) == expectedAsset.TypeObj)
                                    assetIsAcceptable = true;
                                break;
                            case 1:
                                if (
                                    cEconAsset.GetMarketHashName(Config.Cfg.ApiKey)
                                        .ToLower()
                                        .Contains(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 2:
                                if (
                                    cEconAsset.GetMarketHashName(Config.Cfg.ApiKey)
                                        .ToLower()
                                        .StartsWith(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 3:
                                if (cEconAsset.classid == expectedAsset.TypeObj)
                                    assetIsAcceptable = true;
                                break;
                            case 4:
                                if (
                                    Convert.ToUInt32(
                                        marketHandler.GetPriceOverview(Convert.ToUInt32(cEconAsset.appid),
                                            cEconAsset.GetMarketHashName(apiKey)).median_price) >=
                                    Convert.ToUInt32(expectedAsset.TypeObj))
                                    assetIsAcceptable = true;
                                break;
                            case 5:
                                var classid = new Dictionary<string, string>
                                {
                                    {cEconAsset.classid, cEconAsset.instanceid}
                                };
                                AssetClassInfo info = economyHandler.ToAssetClassInfo(
                                    economyHandler.GetAssetClassInfo(apiKey, Convert.ToUInt32(cEconAsset.appid),
                                        classid).result);
                                foreach (Tag tag in info.tags.Where(tag => tag.name == expectedAsset.TypeObj))
                                    assetIsAcceptable = true;

                                break;
                        }
                        break;
                    }
                    if (!assetIsAcceptable) break;
                }
                if (assetIsAcceptable) return acceptableTrade;
            }
            return null;
        }
        */
    }

    //public class Desired_Tags
    //{
    //    public List<Tag> tags { get; set; }
    //}


    #region enums
    public enum TypeIds
    {
        Exact = 0,
        Contains = 1,
        StartsWith = 2,
        ClassId = 3,
        DollarWorth = 4,
        Tag = 5,
        CurrencyTrade = 6
    }

    internal enum EResult
    {
        Invalid = 0,

        OK = 1,
        Fail = 2,
        NoConnection = 3,
        InvalidPassword = 5,
        LoggedInElsewhere = 6,
        InvalidProtocolVer = 7,
        InvalidParam = 8,
        FileNotFound = 9,
        Busy = 10,
        InvalidState = 11,
        InvalidName = 12,
        InvalidEmail = 13,
        DuplicateName = 14,
        AccessDenied = 15,
        Timeout = 16,
        Banned = 17,
        AccountNotFound = 18,
        InvalidSteamID = 19,
        ServiceUnavailable = 20,
        NotLoggedOn = 21,
        Pending = 22,
        EncryptionFailure = 23,
        InsufficientPrivilege = 24,
        LimitExceeded = 25,
        Revoked = 26,
        Expired = 27,
        AlreadyRedeemed = 28,
        DuplicateRequest = 29,
        AlreadyOwned = 30,
        IPNotFound = 31,
        PersistFailed = 32,
        LockingFailed = 33,
        LogonSessionReplaced = 34,
        ConnectFailed = 35,
        HandshakeFailed = 36,
        IOFailure = 37,
        RemoteDisconnect = 38,
        ShoppingCartNotFound = 39,
        Blocked = 40,
        Ignored = 41,
        NoMatch = 42,
        AccountDisabled = 43,
        ServiceReadOnly = 44,
        AccountNotFeatured = 45,
        AdministratorOK = 46,
        ContentVersion = 47,
        TryAnotherCM = 48,
        PasswordRequiredToKickSession = 49,
        AlreadyLoggedInElsewhere = 50,
        Suspended = 51,
        Cancelled = 52,
        DataCorruption = 53,
        DiskFull = 54,
        RemoteCallFailed = 55,
        PasswordNotSet = 56,
        PasswordUnset = 56,
        ExternalAccountUnlinked = 57,
        PSNTicketInvalid = 58,
        ExternalAccountAlreadyLinked = 59,
        RemoteFileConflict = 60,
        IllegalPassword = 61,
        SameAsPreviousValue = 62,
        AccountLogonDenied = 63,
        CannotUseOldPassword = 64,
        InvalidLoginAuthCode = 65,
        AccountLogonDeniedNoMailSent = 66,
        AccountLogonDeniedNoMail = 66,
        HardwareNotCapableOfIPT = 67,
        IPTInitError = 68,
        ParentalControlRestricted = 69,
        FacebookQueryError = 70,
        ExpiredLoginAuthCode = 71,
        IPLoginRestrictionFailed = 72,
        AccountLocked = 73,
        AccountLockedDown = 73,
        AccountLogonDeniedVerifiedEmailRequired = 74,
        NoMatchingURL = 75,
        BadResponse = 76,
        RequirePasswordReEntry = 77,
        ValueOutOfRange = 78,
        UnexpectedError = 79,
        Disabled = 80,
        InvalidCEGSubmission = 81,
        RestrictedDevice = 82,
        RegionLocked = 83,
        RateLimitExceeded = 84,
        AccountLogonDeniedNeedTwoFactorCode = 85,
        AccountLoginDeniedNeedTwoFactor = 85,
        ItemOrEntryHasBeenDeleted = 86,
        ItemDeleted = 86,
        AccountLoginDeniedThrottle = 87,
        TwoFactorCodeMismatch = 88,
        TwoFactorActivationCodeMismatch = 89,
        AccountAssociatedToMultiplePlayers = 90,
        NotModified = 91,
        NoMobileDeviceAvailable = 92,
        TimeIsOutOfSync = 93,
        SMSCodeFailed = 94,
        TooManyAccountsAccessThisResource = 95,
        AccountLimitExceeded = 95,
        AccountActivityLimitExceeded = 96,
        PhoneActivityLimitExceeded = 97
    }

    public enum ETradeOfferState
    {
        ETradeOfferStateInvalid = 1,
        ETradeOfferStateActive = 2,
        ETradeOfferStateAccepted = 3,
        ETradeOfferStateCountered = 4,
        ETradeOfferStateExpired = 5,
        ETradeOfferStateCanceled = 6,
        ETradeOfferStateDeclined = 7,
        ETradeOfferStateInvalidItems = 8,
        ETradeOfferStateEmailPending = 9,
        ETradeOfferStateEmailCanceled = 10
    }

    #endregion

    #region GetTradeOffersSummaryResponse
    [JsonObject(Title = "RootObject")]
    public class GetTradeOffersSummaryBaseResponse
    {
        public GetTradeOffersSummaryResponse response { get; set; }
    }

    [JsonObject(Title = "Response")]
    public class GetTradeOffersSummaryResponse
    {
        public int pending_received_count { get; set; }
        public int new_received_count { get; set; }
        public int updated_received_count { get; set; }
        public int historical_received_count { get; set; }
        public int pending_sent_count { get; set; }
        public int newly_accepted_sent_count { get; set; }
        public int updated_sent_count { get; set; }
        public int historical_sent_count { get; set; }
    }
    #endregion

    [JsonObject(Title = "RootObject")]
    public class MarketValue
    {
        public bool success { get; set; }
        public string lowest_price { get; set; }
        public string volume { get; set; }
        public string median_price { get; set; }
    }

    [JsonObject(Title = "RootObject")]
    public class TradeId
    {
        public ulong tradeid { get; set; }
    }

    [JsonObject(Title = "ItemsToReceive")]
    public class CEconAsset
    {
        public string appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public string classid { get; set; }
        public string instanceid { get; set; }
        public string amount { get; set; }
        public bool missing { get; set; }

        [JsonIgnore] public string name;

        public string GetMarketHashName(string apiKey)
        {
            var _handler = new ISteamEconomyHandler();
            var data = new Dictionary<string, string> { { classid, instanceid } };
            AssetClassInfo info = _handler.ToAssetClassInfo(_handler.GetAssetClassInfo(apiKey, Convert.ToUInt32(appid), data).result);
            return info.market_hash_name;
        }
    }

    [JsonObject(Title = "TradeOffersReceieved")]
    public class CEconTradeOffer
    {
        public string tradeofferid { get; set; }
        public UInt32 accountid_other { get; set; }
        public string message { get; set; }
        public int expiration_time { get; set; }
        public int trade_offer_state { get; set; }
        public List<CEconAsset> items_to_give { get; set; }
        public List<CEconAsset> items_to_receive { get; set; }
        public bool is_our_offer { get; set; }
        public int time_created { get; set; }
        public int time_updated { get; set; }
        public string tradeid { get; set; }
        public bool from_real_time_trade { get; set; }
    }

    [JsonObject(Title = "Response")]
    public class TradeOffersList
    {
        public List<CEconTradeOffer> trade_offers_received { get; set; }
    }

    [JsonObject(Title = "RootObject")]
    public class TradeOffers
    {
        public TradeOffersList response { get; set; }
    }

    [JsonObject(Title = "RootObject")]
    public class SendOfferResponse
    {
        public string tradeofferid { get; set; }
        public bool needs_email_confirmation { get; set; }
        public string email_domain { get; set; }
    }

    public class Me
    {
        public List<CEconAsset> assets = new List<CEconAsset>();
        public List<object> currency = new List<object>();
        public bool ready = false;
    }

    public class Them
    {
        public List<CEconAsset> assets = new List<CEconAsset>();
        public List<object> currency = new List<object>();
        public bool ready = false;
    }

    [JsonObject(Title = "RootObject")]
    public class TradeOffer
    {
        public bool newversion = true;
        public int version = 2;
        public Me me = new Me();
        public Them them = new Them();
    }
}