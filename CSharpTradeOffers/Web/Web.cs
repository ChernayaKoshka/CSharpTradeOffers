using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Web
{
    /// <summary>
    /// Handles Web related tasks like logging in and fetching
    /// </summary>
    public class Web
    {
        /// <summary>
        /// steamcommunity.com
        /// </summary>
        public const string SteamCommunityDomain = "steamcommunity.com";

        private static CookieContainer _cookies = new CookieContainer();

        private readonly IWebRequestHandler<IResponse> _webRequestHandler;

        /// <summary>
        /// Initializes a new Web class in accordance to the IWebRequestHandler IResponse provided.
        /// </summary>
        /// <param name="webRequestHandler"></param>
        public Web(IWebRequestHandler<IResponse> webRequestHandler)
        {
            _webRequestHandler = webRequestHandler;
        }

        /// <summary>
        /// A web method to return the response string from the URL.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">
        /// Special parameter, should only be used with requests that need "X-Requested-With:
        /// XMLHttpRequest" and "X-Prototype-Version: 1.7"
        /// </param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>A string from the response stream.</returns>
        public IResponse Fetch(string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "", bool isWebkit = false)
        {
            IResponse response = _webRequestHandler.HandleWebRequest(url, method, data, cookies, xHeaders, referer, isWebkit);

            return response;
        }

        /// <summary>
        /// Retries a Fetch until success or fail conditions have been met.
        /// </summary>
        /// <param name="retryWait">The TimeSpan in which to wait between requests.</param>
        /// <param name="retryLimit">The max number of requests to perform before failing.</param>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The method to be used. Ex: POST</param>
        /// <param name="data">Dictionary containing the paramters to be sent in the URL or in the Stream, depending on the method.</param>
        /// <param name="cookies">A cookiecontainer with cookies to send.</param>
        /// <param name="xHeaders">
        /// Special parameter, should only be used with requests that need "X-Requested-With:
        /// XMLHttpRequest" and "X-Prototype-Version: 1.7"
        /// </param>
        /// <param name="referer">Sets the referrer for the request.</param>
        /// <returns>IResponse interace.</returns>
        public IResponse RetryFetch(TimeSpan retryWait, int retryLimit, string url, string method,
            Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "", bool isWebkit = false)
        {
            IResponse response = RetryRequestProcessor(retryWait, retryLimit, url, method, data, cookies, xHeaders,
                referer, isWebkit);
            return response;
        }

        /// <summary>
        /// Processes retry attempts
        /// </summary>
        /// <returns>IResponse interace.</returns>
        private IResponse RetryRequestProcessor(TimeSpan retryWait, int retryLimit, string url, string method,
            Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "", bool isWebkit = false)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return Fetch(url, method, data, cookies, xHeaders, referer, isWebkit);
                }
                catch (WebException)
                {
                    if (attempts >= retryLimit)
                        return null;
                    attempts++;
                }

                Thread.Sleep(retryWait);
            }
        }

        /// <summary>
        /// Executes the web login using the Steam website.
        /// </summary>
        /// <param name="username">Username to use</param>
        /// <param name="password">Password to use</param>
        /// <param name="machineAuth">Steam machine auth string. 
        /// <param name="userInputOutput">The user I/O handler to use. If null, defaults to Console.Write/Read</param>
        /// You should save Account.SteamMachineAuth after the code has been entered and the request has 
        /// been successful and pass it to DoLogin in future attempts to login. 
        /// This field can be left blank, but if the account is SteamGuard protected 
        /// it will ask you for a new code every time.</param>
        /// <returns>An Account object to that contains the SteamId and AuthContainer.</returns>
        public Account DoLogin(string username, string password, string machineAuth = "", IUserInputOutputHandler userInputOutput = null)
        {
            if (userInputOutput == null) userInputOutput = new ConsoleInputOutput();

            Thread.Sleep(2000);
            var rsaHelper = new RsaHelper(password);

            var loginDetails = new Dictionary<string, string> { { "username", username } };
            IResponse response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", loginDetails);

            string encryptedBase64Password = rsaHelper.EncryptPassword(response);
            if (encryptedBase64Password == null) return null;

            LoginResult loginJson = null;
            CookieCollection cookieCollection;
            string steamGuardText = string.Empty;
            string steamGuardId = string.Empty;
            string twoFactorText = string.Empty;

            do
            {
                bool captcha = loginJson != null && loginJson.CaptchaNeeded;
                bool steamGuard = loginJson != null && loginJson.EmailAuthNeeded;
                bool twoFactor = loginJson != null && loginJson.RequiresTwofactor;

                var time = Uri.EscapeDataString(rsaHelper.RsaJson.TimeStamp);
                string capGid = "-1";

                if (loginJson != null && loginJson.CaptchaNeeded)
                    capGid = Uri.EscapeDataString(loginJson.CaptchaGid);

                var data = new Dictionary<string, string>
                {
                    {"password", encryptedBase64Password},
                    {"username", username},
                    {"loginfriendlyname", string.Empty},
                    {"remember_login", "false"}
                };
                // Captcha
                string capText = string.Empty;
                if (captcha)
                {
                    Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + loginJson.CaptchaGid);
                    capText =
                        userInputOutput.GetInput(
                            "Please note, if you enter in your captcha correctly and it still opens up new captchas, double check your username and password.\n Please enter the numbers/letters from the picture that opened up: ",
                            "Captcha");
                }

                data.Add("captchagid", capGid);
                data.Add("captcha_text", captcha ? capText : string.Empty);
                // Captcha end

                // SteamGuard
                if (steamGuard)
                {
                    steamGuardText = userInputOutput.GetInput("SteamGuard code required: ", "SteamGuard");
                    steamGuardId = loginJson.EmailSteamId;
                }

                data.Add("emailauth", steamGuardText);
                data.Add("emailsteamid", steamGuardId);
                // SteamGuard end

                //TwoFactor
                if (twoFactor && !loginJson.Success)
                {
                    twoFactorText = userInputOutput.GetInput("TwoFactor code required: ", "Two Factor Authentication");
                }

                data.Add("twofactorcode", twoFactor ? twoFactorText : string.Empty);

                data.Add("rsatimestamp", time);

                CookieContainer cc = null;
                if (!string.IsNullOrEmpty(machineAuth))
                {
                    cc = new CookieContainer();
                    var split = machineAuth.Split('=');
                    var machineCookie = new Cookie(split[0], split[1]);
                    cc.Add(new Uri("https://steamcommunity.com/login/dologin/"), machineCookie);
                }

                using (IResponse webResponse = Fetch("https://steamcommunity.com/login/dologin/", "POST", data, cc))
                {
                    string json = webResponse.ReadStream();
                    loginJson = JsonConvert.DeserializeObject<LoginResult>(json);
                    cookieCollection = webResponse.Cookies;
                }
            } while (loginJson.CaptchaNeeded || loginJson.EmailAuthNeeded || (loginJson.RequiresTwofactor && !loginJson.Success));

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
                            break;
                        case "steamLoginSecure":
                            account.AuthContainer.Add(cookie);
                            break;
                        case "timezoneOffset":
                            account.AuthContainer.Add(cookie);
                            break;
                    }
                    if (cookie.Name.StartsWith("steamMachineAuth"))
                    {
                        account.SteamMachineAuth = cookie.Name + "=" + cookie.Value;
                    }
                    else if (!string.IsNullOrEmpty(machineAuth))
                    {
                        account.SteamMachineAuth = machineAuth;
                    }
                }

                SubmitCookies(_cookies);

                // ReSharper disable once AssignNullToNotNullAttribute
                account.AuthContainer.Add(_cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]);

                return account;
            }
            userInputOutput.OutputMessage("SteamWeb Error: " + loginJson.Message);
            return null;
        }

        /// <summary>
        /// Retries the DoLogin method until success or fail conditions have been met.
        /// </summary>
        /// <param name="retryWait">The TimeSpan in which to wait between requests.</param>
        /// <param name="retryLimit">The max number of requests to perform before failing.</param>
        /// <param name="username">Username to use</param>
        /// <param name="password">Password to use</param>
        /// <param name="machineAuth">Steam machine auth string. 
        ///     You should save Web.SteamMachineAuth after the code has been entered and the request has 
        ///     been successful and pass it to DoLogin in future attempts to login. 
        ///     This field can be left blank, but if the account is SteamGuard protected 
        ///     it will ask you for a new code every time.</param>
        /// <returns></returns>
        public Account RetryDoLogin(TimeSpan retryWait, int retryLimit, string username, string password,
            string machineAuth = "", IUserInputOutputHandler userInputOutput = null)
        {
            if (userInputOutput == null) userInputOutput = new ConsoleInputOutput();

            int retries = 0;
            Account account = null;
            do
            {
                try
                {
                    account = DoLogin(username, password, machineAuth, userInputOutput);
                }
                catch (WebException)
                {
                    retries++;
                    if (retries == retryLimit) throw;
                    Thread.Sleep(retryWait);
                }
            } while (account == null);
            return account;
        }

        private static void SubmitCookies(CookieContainer cookies)
        {
            var w = WebRequest.Create("https://steamcommunity.com/") as HttpWebRequest;

            if (w == null) return;

            w.Method = "POST";
            w.ContentType = "application/x-www-form-urlencoded";
            w.CookieContainer = cookies;
            w.ContentLength = 0;

            w.GetResponse().Close();
        }
    }
}