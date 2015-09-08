using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Cache;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Handles Web related tasks like logging in and fetching
    /// </summary>
    public class Web
    {
        public const string SteamCommunityDomain = "steamcommunity.com";

        public static string SteamLogin { get; private set; }

        public static string SessionId { get; private set; }

        public static string SteamLoginSecure { get; private set; }

        public static string SteamMachineAuth { get; private set; }

        public static string TimezoneOffset { get; private set; }

        private static CookieContainer _cookies = new CookieContainer();

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
        public static HttpWebResponse Request(string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            bool isGetMethod = (method.ToLower() == "get");
            string dataString = null;

            if (data != null)
                dataString = "?" + DictionaryToUrlString(data);

            if (isGetMethod && !string.IsNullOrEmpty(dataString))
            {
                url += dataString;
            }

            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Method = method;
                request.Accept = "application/json, text/javascript;q=0.9, */*;q=0.5";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
                request.Referer = string.IsNullOrEmpty(referer) ? "http://steamcommunity.com/" : referer;
                request.Timeout = 50000;
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                if (xHeaders)
                {
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    request.Headers.Add("X-Prototype-Version", "1.7");
                }
                request.CookieContainer = cookies ?? new CookieContainer();

                if (data != null && !isGetMethod)
                {
                    //string dataString = DictionaryToURLString(data);
                    byte[] dataBytes = Encoding.UTF8.GetBytes(DictionaryToUrlString(data));
                    request.ContentLength = dataBytes.Length;

                    using (var reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(dataBytes, 0, dataBytes.Length);
                    }
                }
                return request.GetResponse() as HttpWebResponse;
            }
            return null;
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
        public static string Fetch(string url, string method, Dictionary<string, string> data = null, CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            HttpWebResponse response = Request(url, method, data, cookies, xHeaders, referer);

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
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
        public static string RetryFetch(int retryWait, int retryCount, string url, string method, Dictionary<string, string> data = null,
            CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    var response = Request(url, method, data, cookies, xHeaders, referer);

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
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
        public static Stream RetryFetchStream(int retryWait, int retryCount, string url, string method, Dictionary<string, string> data = null,
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
        public static Stream FetchStream(string url, string method, Dictionary<string, string> data = null, CookieContainer cookies = null, bool xHeaders = true, string referer = "")
        {
            return Request(url, method, data, cookies, xHeaders, referer).GetResponseStream();
        }

        /// <summary>
        /// Converts a dictionary to URL parameters. Ex: ?param=arg
        /// </summary>
        /// <param name="dict">The Dictionary to be converted.</param>
        /// <returns>A concatenated string of URL arguments.</returns>
        public static string DictionaryToUrlString(Dictionary<string, string> dict)
        {
            string joined = "";
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                if (joined != "")
                    joined += "&";

                //joined += $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}";
                joined += $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}";
            }
            return joined;
        }

        /// <summary>
        /// Executes the login by using the Steam Website.
        /// </summary> 
        public static Account DoLogin(string username, string password, string machineAuth = "")
        {
            Thread.Sleep(2000);
            RsaHelper rsaHelper = new RsaHelper(username, password);
            string encryptedBase64Password = rsaHelper.EncryptPassword();
            if(encryptedBase64Password == null) return null;
            
            DoLoginResult loginJson = null;
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

                using (var webResponse = Request("https://steamcommunity.com/login/dologin/", "POST", data, cc))
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        loginJson = JsonConvert.DeserializeObject<DoLoginResult>(json);
                        cookieCollection = webResponse.Cookies;
                    }
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

        static void SubmitCookies(CookieContainer cookies)
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

        public class RsaHelper
        {
            private readonly string _username;

            private readonly string _password;

            public RsaHelper(string username, string password)
            {
                _username = username;
                _password = password;
            }

            public bool RequestRsaKey()
            {
                var data = new Dictionary<string, string> { { "username", _username } };
                string response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", data);
                GetRsaKey rsaJson = JsonConvert.DeserializeObject<GetRsaKey>(response);
                if (!rsaJson.Success) return false;
                RsaJson = rsaJson;
                return true;
            }

            public string EncryptPassword()
            {
                if (!RequestRsaKey()) return null;

                //RSA Encryption
                var rsa = new RSACryptoServiceProvider();
                var rsaParameters = new RSAParameters
                {
                    Exponent = HexToByte(RsaJson.PublicKeyExp),
                    Modulus = HexToByte(RsaJson.PublicKeyMod)
                };


                rsa.ImportParameters(rsaParameters);

                byte[] bytePassword = Encoding.ASCII.GetBytes(_password);
                byte[] encodedPassword = rsa.Encrypt(bytePassword, false);
                return Convert.ToBase64String(encodedPassword);
            }

            private static byte[] HexToByte(string hex)
            {
                if (hex.Length % 2 == 1)
                    throw new Exception("The binary key cannot have an odd number of digits");

                byte[] arr = new byte[hex.Length >> 1];
                int l = hex.Length;

                for (int i = 0; i < (l >> 1); ++i)
                {
                    arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
                }

                return arr;
            }

            private static int GetHexVal(char hex)
            {
                int val = hex;
                return val - (val < 58 ? 48 : 55);
            }

            public GetRsaKey RsaJson { get; private set; }
        }

        // JSON Classes

        public class GetRsaKey
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("publickey_mod")]
            public string PublicKeyMod { get; set; }
            [JsonProperty("publickey_exp")]
            public string PublicKeyExp { get; set; }
            [JsonProperty("timestamp")]
            public string TimeStamp { get; set; }
        }
        

        [JsonObject(Title = "RootObject")]
        public class DoLoginResult
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("requires_twofactor")]
            public bool RequiresTwofactor { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("captcha_needed")]
            public bool CaptchaNeeded { get; set; }
            [JsonProperty("captcha_gid")]
            public string CaptchaGid { get; set; }
            [JsonProperty("emailauth_needed")]
            public bool EmailAuthNeeded { get; set; }
            [JsonProperty("emaildomain")]
            public string EmailDomain { get; set; }
            [JsonProperty("emailsteamid")]
            public string EmailSteamId { get; set; }
            [JsonProperty("login_complete")]
            public bool LoginComplete { get; set; }
            [JsonProperty("transfer_url")]
            public string TransferUrl { get; set; }
            [JsonProperty("transfer_parameters")]
            public TransferParameters TransferParameters { get; set; }
        }

        [JsonObject(Title = "Transfer_Parameters")]
        public class TransferParameters
        {
            [JsonProperty("steamid")]
            public string Steamid { get; set; }
            [JsonProperty("token")]
            public string Token { get; set; }
            [JsonProperty("auth")]
            public string Auth { get; set; }
            [JsonProperty("remember_login")]
            public bool RememberLogin { get; set; }
            [JsonProperty("token_secure")]
            public string TokenSecure { get; set; }
        }
    }
}
