using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Cache;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using CSharpTradeOffers.Configuration;

namespace CSharpTradeOffers
{
    /// <summary>
    /// Handles Web related tasks like logging in and fetching
    /// </summary>
    public class Web
    {

        public const string SteamCommunityDomain = "steamcommunity.com";


        public static string SteamLogin { get; set; }


        public static string SessionId { get; set; }


        public static string SteamLoginSecure { get; set; }


        public static string SteamMachineAuth { get; set; }


        public static string TimezoneOffset { get; set; }

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
        public static string Fetch(string url, string method, Dictionary<string,string> data = null, CookieContainer cookies = null, bool xHeaders = true, string referer = "" )
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
        public static string RetryFetch(string url, string method, Dictionary<string, string> data = null,
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
                    if (attempts >= 5)
                        return null;
                    attempts++;
                }
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
                joined += string.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key),
                    WebUtility.UrlEncode(kvp.Value));
            }
            return joined;
        }

        #region from JC96
        /// <summary>
        /// Executes the login by using the Steam Website.
        /// </summary> 
        public static bool DoLogin(string username, string password, ref Account account, ref RootConfig cfg)
        {
            Thread.Sleep(2000);
            var data = new Dictionary<string, string> {{"username", username}};
            string response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", data);
            GetRsaKey rsaJson = JsonConvert.DeserializeObject<GetRsaKey>(response);

            // Validate
            if (!rsaJson.success)
            {
                return false;
            }

            //RSA Encryption
            var rsa = new RSACryptoServiceProvider();
            var rsaParameters = new RSAParameters
            {
                Exponent = HexToByte(rsaJson.publickey_exp),
                Modulus = HexToByte(rsaJson.publickey_mod)
            };


            rsa.ImportParameters(rsaParameters);

            byte[] bytePassword = Encoding.ASCII.GetBytes(password);
            byte[] encodedPassword = rsa.Encrypt(bytePassword, false);
            string encryptedBase64Password = Convert.ToBase64String(encodedPassword);

            SteamResult loginJson = null;
            CookieCollection cookieCollection;
            string steamGuardText = "";
            string steamGuardId = "";
            do
            {
                bool captcha = loginJson != null && loginJson.captcha_needed;
                bool steamGuard = loginJson != null && loginJson.emailauth_needed;

                var time = Uri.EscapeDataString(rsaJson.timestamp);
                var capGid = "-1";
                
                if(loginJson != null && loginJson.captcha_needed)
                    capGid = Uri.EscapeDataString(loginJson.captcha_gid);

                data = new Dictionary<string, string>
                {
                    {"password", encryptedBase64Password},
                    {"username", username},
                    {"twofactorcode", ""},
                    {"loginfriendlyname", ""},
                    {"rememberlogin", "false"}
                };
                // Captcha
                string capText = "";
                if (captcha)
                {
                    System.Diagnostics.Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + loginJson.captcha_gid);
                    Console.Write("Please enter the numbers/letters from the picture that opened up: ");
                    capText = Uri.EscapeDataString(Console.ReadLine());
                }

                data.Add("captchagid", captcha ? capGid : "");
                data.Add("captcha_text", captcha ? capText : "");
                // Captcha end

                // SteamGuard
                if (steamGuard)
                {
                    Console.Write("SteamGuard code required: ");
                    steamGuardText = Uri.EscapeDataString(Console.ReadLine());
                    steamGuardId = loginJson.emailsteamid;
                }

                data.Add("emailauth", steamGuardText);
                data.Add("emailsteamid", steamGuardId);
                // SteamGuard end

                data.Add("rsatimestamp", time);

                CookieContainer cc = null;
                if(cfg.SteamMachineAuth != null && cfg.SteamMachineAuth != "null" && cfg.SteamMachineAuth != "")
                {
                    cc = new CookieContainer();
                    var split = cfg.SteamMachineAuth.Split('=');
                    var machineCookie = new Cookie(split[0], split[1]);
                    cc.Add(new Uri("https://steamcommunity.com/login/dologin/"), machineCookie);
                }


                using (var webResponse = Request("https://steamcommunity.com/login/dologin/", "POST", data, cc))
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        loginJson = JsonConvert.DeserializeObject<SteamResult>(json);
                        cookieCollection = webResponse.Cookies;
                    }
                }
            } while (loginJson.captcha_needed || loginJson.emailauth_needed);


            if (loginJson.success)
            {
                _cookies = new CookieContainer();
                foreach (Cookie cookie in cookieCollection)
                {
                    _cookies.Add(cookie);
                    switch(cookie.Name)
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
                    account.AuthContainer.Add(cookie);
                    SteamMachineAuth = cookie.Name + "=" + cookie.Value;
                }
                SubmitCookies(_cookies);
                account.AuthContainer.Add(_cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]);
                SessionId = _cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"].Value;
                return true;
            }
            Console.WriteLine("SteamWeb Error: " + loginJson.message);
            return false;
        }

        /// <summary>
        /// Executes the login by using the Steam Website.
        /// </summary>   
        public static bool DoLogin(string username, string password, ref Account account)
        {
            Thread.Sleep(2000);
            var data = new Dictionary<string, string> { { "username", username } };
            string response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", data, null);
            GetRsaKey rsaJson = JsonConvert.DeserializeObject<GetRsaKey>(response);

            // Validate
            if (!rsaJson.success)
            {
                return false;
            }

            //RSA Encryption
            var rsa = new RSACryptoServiceProvider();
            var rsaParameters = new RSAParameters
            {
                Exponent = HexToByte(rsaJson.publickey_exp),
                Modulus = HexToByte(rsaJson.publickey_mod)
            };


            rsa.ImportParameters(rsaParameters);

            byte[] bytePassword = Encoding.ASCII.GetBytes(password);
            byte[] encodedPassword = rsa.Encrypt(bytePassword, false);
            string encryptedBase64Password = Convert.ToBase64String(encodedPassword);

            SteamResult loginJson = null;
            CookieCollection cookieCollection;
            string steamGuardText = "";
            string steamGuardId = "";
            do
            {
                bool captcha = loginJson != null && loginJson.captcha_needed;
                bool steamGuard = loginJson != null && loginJson.emailauth_needed;

                var time = Uri.EscapeDataString(rsaJson.timestamp);
                var capGid = "-1";

                if (loginJson != null && loginJson.captcha_needed)
                    capGid = Uri.EscapeDataString(loginJson.captcha_gid);

                data = new Dictionary<string, string>
                {
                    {"password", encryptedBase64Password},
                    {"username", username},
                    {"twofactorcode", ""},
                    {"loginfriendlyname", ""},
                    {"rememberlogin", "false"}
                };
                // Captcha
                string capText = "";
                if (captcha)
                {
                    System.Diagnostics.Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + loginJson.captcha_gid);
                    Console.Write("Please enter the numbers/letters from the picture that opened up: ");
                    capText = Uri.EscapeDataString(Console.ReadLine());
                }

                data.Add("captchagid", captcha ? capGid : "");
                data.Add("captcha_text", captcha ? capText : "");
                // Captcha end

                // SteamGuard
                if (steamGuard)
                {
                    Console.Write("SteamGuard code required: ");
                    steamGuardText = Uri.EscapeDataString(Console.ReadLine());
                    steamGuardId = loginJson.emailsteamid;
                }

                data.Add("emailauth", steamGuardText);
                data.Add("emailsteamid", steamGuardId);
                // SteamGuard end

                data.Add("rsatimestamp", time);

                CookieContainer cc = null;

                using (var webResponse = Request("https://steamcommunity.com/login/dologin/", "POST", data, cc))
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        loginJson = JsonConvert.DeserializeObject<SteamResult>(json);
                        cookieCollection = webResponse.Cookies;
                    }
                }
            } while (loginJson.captcha_needed || loginJson.emailauth_needed);


            if (loginJson.success)
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

                    account.AuthContainer.Add(cookie);
                    SteamMachineAuth = cookie.Name + "=" + cookie.Value;
                }
                SubmitCookies(_cookies);

                account.AuthContainer.Add(_cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]);

                SessionId = _cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"].Value;

                return true;
            }
            Console.WriteLine("SteamWeb Error: " + loginJson.message);
            return false;
        }

        /// <summary>
        /// Executes the login by using the Steam Website.
        /// </summary> 
        public static bool DoLogin(string username, string password, ref Account account, string machineAuth)
        {
            Thread.Sleep(2000);
            var data = new Dictionary<string, string> { { "username", username } };
            string response = Fetch("https://steamcommunity.com/login/getrsakey", "POST", data);
            GetRsaKey rsaJson = JsonConvert.DeserializeObject<GetRsaKey>(response);

            // Validate
            if (!rsaJson.success) return false;

            //RSA Encryption
            var rsa = new RSACryptoServiceProvider();
            var rsaParameters = new RSAParameters
            {
                Exponent = HexToByte(rsaJson.publickey_exp),
                Modulus = HexToByte(rsaJson.publickey_mod)
            };


            rsa.ImportParameters(rsaParameters);

            byte[] bytePassword = Encoding.ASCII.GetBytes(password);
            byte[] encodedPassword = rsa.Encrypt(bytePassword, false);
            string encryptedBase64Password = Convert.ToBase64String(encodedPassword);

            SteamResult loginJson = null;
            CookieCollection cookieCollection;
            string steamGuardText = "";
            string steamGuardId = "";
            do
            {
                bool captcha = loginJson != null && loginJson.captcha_needed;
                bool steamGuard = loginJson != null && loginJson.emailauth_needed;

                var time = Uri.EscapeDataString(rsaJson.timestamp);
                var capGid = "-1";

                if (loginJson != null && loginJson.captcha_needed)
                    capGid = Uri.EscapeDataString(loginJson.captcha_gid);

                data = new Dictionary<string, string>
                {
                    {"password", encryptedBase64Password},
                    {"username", username},
                    {"twofactorcode", ""},
                    {"loginfriendlyname", ""},
                    {"rememberlogin", "false"}
                };
                // Captcha
                string capText = "";
                if (captcha)
                {
                    System.Diagnostics.Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + loginJson.captcha_gid);
                    Console.Write("Please enter the numbers/letters from the picture that opened up: ");
                    capText = Uri.EscapeDataString(Console.ReadLine());
                }

                data.Add("captchagid", captcha ? capGid : "");
                data.Add("captcha_text", captcha ? capText : "");
                // Captcha end

                // SteamGuard
                if (steamGuard)
                {
                    Console.Write("SteamGuard code required: ");
                    steamGuardText = Uri.EscapeDataString(Console.ReadLine());
                    steamGuardId = loginJson.emailsteamid;
                }

                data.Add("emailauth", steamGuardText);
                data.Add("emailsteamid", steamGuardId);
                // SteamGuard end

                data.Add("rsatimestamp", time);

                CookieContainer cc = null;
                if (machineAuth != null)
                {
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
                        loginJson = JsonConvert.DeserializeObject<SteamResult>(json);
                        cookieCollection = webResponse.Cookies;
                    }
                }
            } while (loginJson.captcha_needed || loginJson.emailauth_needed);


            if (loginJson.success)
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
                    account.AuthContainer.Add(cookie);
                    SteamMachineAuth = cookie.Name + "=" + cookie.Value;
                }

                SubmitCookies(_cookies);

                account.AuthContainer.Add(_cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"]);

                SessionId = _cookies.GetCookies(new Uri("https://steamcommunity.com"))["sessionid"].Value;

                return true;
            }
            Console.WriteLine("SteamWeb Error: " + loginJson.message);
            return false;
        }

        static void SubmitCookies(CookieContainer cookies)
        {
            var w = WebRequest.Create("https://steamcommunity.com/") as HttpWebRequest;

            w.Method = "POST";
            w.ContentType = "application/x-www-form-urlencoded";
            w.CookieContainer = cookies;
            w.ContentLength = 0;

            w.GetResponse().Close();
        }

        static byte[] HexToByte(string hex)
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

        static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : 55);
        }
        #endregion

        // JSON Classes

        public class GetRsaKey
        {
            public bool success { get; set; }
            public string publickey_mod { get; set; }
            public string publickey_exp { get; set; }
            public string timestamp { get; set; }
        }


        public class SteamResult
        {
            public bool success { get; set; }
            public bool requires_twofactor { get; set; }
            public string message { get; set; }
            public bool captcha_needed { get; set; }
            public string captcha_gid { get; set; }
            public bool emailauth_needed { get; set; }
            public string emaildomain { get; set; }
            public string emailsteamid { get; set; }
        }
    }
}
