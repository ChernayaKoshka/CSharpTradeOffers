using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Web;
using Newtonsoft.Json;
using SteamAuth;
using LoginResult = SteamAuth.LoginResult;

namespace AcceptAllMobileConfirmations
{
    internal class Program
    {
        private static Account _account;
        private static DefaultConfig _config = new DefaultConfig();
        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        private static void Main()
        {
            Console.Title = "Confirmer by sne4kyFox";
            Console.WriteLine("This program will accept any and ALL mobile auth confirmations, use with extreme caution.");
            Console.WriteLine(
                "By using this software you agree to the terms in \"license.txt\".");

            _config = ConfigHandler.Reload();

            Console.WriteLine("Attempting web login...");

            _account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, _config.Username, _config.Password, _config.SteamMachineAuth);

            if (!string.IsNullOrEmpty(_account.SteamMachineAuth))
            {
                _config.SteamMachineAuth = _account.SteamMachineAuth;
                ConfigHandler.WriteChanges(_config);
            }

            Console.WriteLine("Login was successful!");
            while (!File.Exists("account.maFile"))
                SteamAuthLogin();
            var sgAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(File.ReadAllText("account.maFile"));

            AcceptConfirmationsLoop(sgAccount);
        }

        private static void SteamAuthLogin()
        {
            var authLogin = new UserLogin(_config.Username, _config.Password);
            LoginResult result;
            do
            {
                result = authLogin.DoLogin();
                switch (result)
                {
                    case LoginResult.NeedEmail:
                        Console.Write("An email was sent to this account's address, please enter the code here to continue: ");
                        authLogin.EmailCode = Console.ReadLine();
                        break;
                    case LoginResult.NeedCaptcha:
                        Process.Start("https://steamcommunity.com/public/captcha.php?gid=" + authLogin.CaptchaGID);
                        Console.Write("Please enter the captcha that just opened up on your default browser: ");
                        authLogin.CaptchaText = Console.ReadLine();
                        break;
                    case LoginResult.Need2FA:
                        Console.Write("Please enter in your authenticator code: ");
                        authLogin.TwoFactorCode = Console.ReadLine();
                        break;
                    default:
                        throw new Exception("Case was not accounted for. Case: " + result);
                }
            } while (result != LoginResult.LoginOkay);

            AuthenticatorLinker linker = new AuthenticatorLinker(authLogin.Session);
            Console.Write("Please enter the number you wish to associate this account in the format +1XXXXXXXXXX where +1 is your country code, leave blank if no new number is desired: ");
             
            string phoneNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phoneNumber)) phoneNumber = null;
            linker.PhoneNumber = phoneNumber;

            AuthenticatorLinker.LinkResult linkResult = linker.AddAuthenticator();
            if (linkResult != AuthenticatorLinker.LinkResult.AwaitingFinalization)
            {
                Console.WriteLine("Could not add authenticator: " + linkResult);
                Console.WriteLine(
                    "If you attempted to link an already linked account, please tell FatherFoxxy to get off his ass and implement the new stuff.");
                return;
            }

            if (!SaveMobileAuth(linker))
            {
                Console.WriteLine("Issue saving auth file, link operation abandoned.");
                return;
            }

            Console.WriteLine(
                "You should have received an SMS code, please input it here. If the code does not arrive, please input a blank line to abandon the operation.");
            AuthenticatorLinker.FinalizeResult finalizeResult;
            do
            {
                Console.Write("SMS Code: ");
                string smsCode = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(smsCode)) return;
                finalizeResult = linker.FinalizeAddAuthenticator(smsCode);

            } while (finalizeResult != AuthenticatorLinker.FinalizeResult.BadSMSCode);
        }

        private static bool SaveMobileAuth(AuthenticatorLinker linker)
        {
            try
            {
                string sgFile = JsonConvert.SerializeObject(linker.LinkedAccount, Formatting.Indented);
                const string fileName = "account.maFile";
                File.WriteAllText(fileName, sgFile);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static void AcceptConfirmationsLoop(SteamGuardAccount sgAccount)
        {
            sgAccount.Session.SteamLogin = _account.FindCookieByName("steamlogin").Value;
            sgAccount.Session.SteamLoginSecure = _account.FindCookieByName("steamloginsecure").Value;
            while (true) //permanent loop, can be changed 
            {
                Thread.Sleep(10000);

                Console.WriteLine(sgAccount.GenerateSteamGuardCode());

                foreach (Confirmation confirmation in sgAccount.FetchConfirmations())
                {
                    sgAccount.AcceptConfirmation(confirmation);
                }
            }
        }
    }
}
