using System;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using CSharpTradeOffers.Json;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Handles Web related tasks like logging in and fetching
    /// </summary>
    public class Web
    {
        private readonly IRequestHandler<IResponse> _requestHandler;
        public const string SteamCommunityDomain = "steamcommunity.com";

        public static string SteamLogin { get; private set; }

        public static string SessionId { get; private set; }

        public static string SteamLoginSecure { get; private set; }

        public static string SteamMachineAuth { get; private set; }

        public static string TimezoneOffset { get; private set; }

        private static CookieContainer _cookies = new CookieContainer();

        public Web(IRequestHandler<IResponse> requestHandler)
        {
            _requestHandler = requestHandler;
        }

        /// <summary>
        /// Fetch calls this method.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary> containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">Special parameter, should only be used with requests that need "X-Requested-With: XMLHttpRequest" and "X-Prototype-Version: 1.7"</param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>An HttpWebResponse object from the requested URL.</returns>
        public IResponse Request(string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            return _requestHandler.HandleRequest(url, method, data, cookies, xHeaders, referer);
        }

        /// <summary>
        /// A web method to return the response string from the URL.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">Special parameter, should only be used with requests that need "X-Requested-With: XMLHttpRequest" and "X-Prototype-Version: 1.7"</param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>A string from the response stream.</returns>
        public string Fetch(string url, string method, Dictionary<string, string> data = null, CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            IResponse response = Request(url, method, data, cookies, xHeaders, referer);

            return response.GetResponseStream().ReadToEnd();
        }

        /// <summary>
        /// A web method to return the response string from the URL.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">Special parameter, should only be used with requests that need "X-Requested-With: XMLHttpRequest" and "X-Prototype-Version: 1.7"</param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>A string from the response stream.</returns>
        public string RetryFetch(TimeSpan retryWait, int retryCount, string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    IResponse response = Request(url, method, data, cookies, xHeaders, referer);

                    ISteamStream responseStream = response.GetResponseStream();

                    return responseStream.ReadToEnd();
                }
                catch (WebException)
                {
                    if (attempts >= retryCount)
                        return null;
                    attempts++;
                }
                Thread.Sleep(retryWait);
            }
        }

        /// <summary>
        /// A web method to return the response string from the URL.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">Special parameter, should only be used with requests that need "X-Requested-With: XMLHttpRequest" and "X-Prototype-Version: 1.7"</param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>A string from the response stream.</returns>
        public ISteamStream RetryFetchStream(TimeSpan retryWait, int retryCount, string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return Request(url, method, data, cookies, xHeaders, referer).GetResponseStream();

                }
                catch (WebException)
                {
                    if (attempts >= retryCount)
                        return null;
                    attempts++;
                }
                Thread.Sleep(retryWait);
            }
        }

        /// <summary>
        /// A web method to return the response string from the URL.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">Special parameter, should only be used with requests that need "X-Requested-With: XMLHttpRequest" and "X-Prototype-Version: 1.7"</param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>The response stream.</returns>
        public ISteamStream FetchStream(string url, string method, Dictionary<string, string> data = null, CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            return Request(url, method, data, cookies, xHeaders, referer).GetResponseStream();
        }

        /// <summary>
        /// Executes the login by using the Steam Website.
        /// </summary> 
        public Account DoLogin(string username, string password, string machineAuth = "")
        {
            Thread.Sleep(2000);
            RsaHelper rsaHelper = new RsaHelper(password);

            var loginDetails = new Dictionary<string, string> {{"username", username}};
            string response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", loginDetails);

            string encryptedBase64Password = rsaHelper.EncryptPasswordResponse(response);
            if (encryptedBase64Password == null) return null;

            LoginResult loginJson = null;
            CookieCollection cookieCollection;
            string steamGuardText = "";
            string steamGuardId = "";
            string twoFactorText = "";

            do
            {
                bool captcha = loginJson != null && loginJson.CaptchaNeeded;
                bool steamGuard = loginJson != null && loginJson.EmailAuthNeeded;
                bool twoFactor = loginJson != null && loginJson.RequiresTwofactor;

                var time = Uri.EscapeDataString(rsaHelper.RsaJson.TimeStamp);
                var capGid = "-1";

                if (loginJson != null && loginJson.CaptchaNeeded)
                    capGid = Uri.EscapeDataString(loginJson.CaptchaGid);

                var data = new Dictionary<string, string>
                {
                    {"password", encryptedBase64Password},
                    {"username", username},
                    {"loginfriendlyname", ""},
                    {"rememberlogin", "false"}
                };
                // Captcha
                string capText = "";
                if (captcha)
                {
                    System.Diagnostics.Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + loginJson.CaptchaGid);
                    Console.WriteLine("Please note, if you enter in your captcha correctly and it still opens up new captchas, double check your username and password.");
                    Console.Write("Please enter the numbers/letters from the picture that opened up: ");
                    capText = Console.ReadLine();
                }

                data.Add("captchagid", captcha ? capGid : "");
                data.Add("captcha_text", captcha ? capText : "");
                // Captcha end

                // SteamGuard
                if (steamGuard)
                {
                    Console.Write("SteamGuard code required: ");
                    steamGuardText = Console.ReadLine();
                    steamGuardId = loginJson.EmailSteamId;
                }

                data.Add("emailauth", steamGuardText);
                data.Add("emailsteamid", steamGuardId);
                // SteamGuard end

                //TwoFactor
                if (twoFactor)
                {
                    Console.WriteLine("TwoFactor code required: ");
                    twoFactorText = Console.ReadLine();
                }

                data.Add("twofactorcode", twoFactor ? twoFactorText : "");

                data.Add("rsatimestamp", time);

                CookieContainer cc = null;
                if (!string.IsNullOrEmpty(machineAuth))
                {
                    SteamMachineAuth = machineAuth;
                    cc = new CookieContainer();
                    var split = machineAuth.Split('=');
                    var machineCookie = new Cookie(split[0], split[1]);
                    cc.Add(new Uri("https://steamcommunity.com/login/dologin/"), machineCookie);
                }

                using (IResponse webResponse = Request("https://steamcommunity.com/login/dologin/", "POST", data, cc))
                {
                    var steamStream = webResponse.GetResponseStream();

                    string json = steamStream.ReadToEnd();
                    loginJson = JsonConvert.DeserializeObject<LoginResult>(json);
                    cookieCollection = webResponse.Cookies;
                }
            } while (loginJson.CaptchaNeeded || loginJson.EmailAuthNeeded || loginJson.RequiresTwofactor);

            Account account;

            if (loginJson.EmailSteamId != null)
                account = new Account(Convert.ToUInt64(loginJson.EmailSteamId));
            else if (loginJson.TransferParameters?.Steamid != null)
                account = new Account(Convert.ToUInt64(loginJson.TransferParameters.Steamid));
            else
                return null;

            if (loginJson.Success)
            {
                _cookies = new CookieContainer();
                foreach (Cookie cookie in cookieCollection)
                {
                    _cookies.Add(cookie);
                    switch (cookie.Name)
                    {
                        case "steamLogin":
                            account.AuthContainer.Add(cookie);
                            SteamLogin = cookie.Value;
                            break;
                        case "steamLoginSecure":
                            account.AuthContainer.Add(cookie);
                            SteamLoginSecure = cookie.Value;
                            break;
                        case "timezoneOffset":
                            account.AuthContainer.Add(cookie);
                            TimezoneOffset = cookie.Value;
                            break;
                    }
                    if (!cookie.Name.StartsWith("steamMachineAuth")) continue;
                    SteamMachineAuth = cookie.Name + "=" + cookie.Value;
                }

                if (!string.IsNullOrEmpty(SteamMachineAuth))
                    account.AddMachineAuthCookies(SteamMachineAuth);

                SubmitCookies(_cookies);

                account.AuthContainer.Add(_cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]);

                SessionId = _cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]?.Value;

                return account;
            }
            Console.WriteLine("SteamWeb Error: " + loginJson.Message);
            return null;
        }

        public Account RetryDoLogin(string username, string password, string machineAuth = "",
            int retryLimit = 10, int retryWait = 500)
        {
            int retries = 0;
            Account account = null;
            do
            {
                try
                {
                    account = DoLogin(username, password, machineAuth);
                }
                catch (WebException)
                {
                    retries++;
                    if (retries == retryLimit) throw;
                    Console.WriteLine("Connection failed... retrying in: " + retryWait + "ms. Retries: " + retries);
                    Thread.Sleep(retryWait);
                }
            } while (account == null);
            return account;
        }

        private static void SubmitCookies(CookieContainer cookies)
        {
            var w = WebRequest.Create("https://steamcommunity.com/") as HttpWebRequest;

            if (w != null)
            {
                w.Method = "POST";
                w.ContentType = "application/x-www-form-urlencoded";
                w.CookieContainer = cookies;
                w.ContentLength = 0;

                w.GetResponse().Close();
            }
        }
    }
}