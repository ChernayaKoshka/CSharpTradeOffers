using System;
using System.Collections.Generic;
using System.Threading;
using CSharpTradeOffers;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Trading;

namespace DonationBot
{
    internal class Program
    {
        private static string _user, _pass;
        private static string _apiKey;
        private static Account _account;
        private static Config _config = new Config();
        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        private static void Main()
        {
            Console.Title = "Donation Bot by sne4kyFox";
            Console.WriteLine("Welcome to the donation bot!");
            Console.WriteLine(
                "By using this software you agree to the terms in \"license.txt\".");

            _config = ConfigHandler.Reload();

            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                Console.WriteLine("Fatal error: API key is missing. Please fill in the API key field in \"configuration.xml\"");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            _apiKey = _config.ApiKey;

            if (string.IsNullOrEmpty(_config.Username) || string.IsNullOrEmpty(_config.Password))
            {
                Console.WriteLine("Please input your username and password.");

                Console.Write("Username: ");
                _user = Console.ReadLine();

                Console.Write("Password: ");
                _pass = Console.ReadLine();
            }
            else
            {
                _user = _config.Username;
                _pass = _config.Password;
            }

            Console.WriteLine("Attempting web login...");

            _account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, _user, _pass, _config.SteamMachineAuth);

            if (!string.IsNullOrEmpty(Web.SteamMachineAuth))
            {
                _config.SteamMachineAuth = Web.SteamMachineAuth;
                ConfigHandler.WriteChanges(_config);
            }

            Console.WriteLine("Login was successful!");

            PollOffers();
        }

        private static void PollOffers()
        {
            Console.WriteLine("Polling offers every ten seconds.");

            bool isPolling = true;

            var offerHandler = new EconServiceHandler(_apiKey);
            var marketHandler = new MarketHandler();

            marketHandler.EligibilityCheck(_account.SteamId, _account.AuthContainer);
            //required to perform trades (?). Checks to see whether or not we're allowed to trade.

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (isPolling) //permanent loop, can be changed 
            {
                Thread.Sleep(10000);

                var recData = new Dictionary<string, string>
                {
                    {"get_received_offers", "1"},
                    {"active_only", "1"},
                    {"time_historical_cutoff", "999999999999"}
                    //arbitrarily high number to retrieve latest offers
                };

                var offers = offerHandler.GetTradeOffers(recData).TradeOffersReceived;

                if (offers == null) continue;

                foreach (CEconTradeOffer cEconTradeOffer in offers)
                {
                    if (cEconTradeOffer.ItemsToGive == null)
                    {
                        offerHandler.AcceptTradeOffer(Convert.ToUInt64(cEconTradeOffer.TradeOfferId),
                            _account.AuthContainer,
                            cEconTradeOffer.AccountIdOther, "1");
                        Console.WriteLine("Accepted a donation!");
                    }
                    else
                    {
                        offerHandler.DeclineTradeOffer(Convert.ToUInt64(cEconTradeOffer.TradeOfferId));
                        Console.WriteLine("Refused a \"donation\" that would have taken items from us.");
                    }
                }
            }
        }
    }
}
