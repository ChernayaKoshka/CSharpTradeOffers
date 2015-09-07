using System;
using System.Collections.Generic;
using System.Threading;
using CSharpTradeOffers;
using CSharpTradeOffers.Trading;

namespace DonationBot
{
    internal class Program
    {
        private static string _user, _pass;
        private static string _apiKey;
        private static Account _account;
        private static readonly Config Cfg = new Config();

        private static void Main()
        {
            Console.Title = "Donation Bot by sne4kyFox";
            Console.WriteLine("Welcome to the donation bot!");
            Console.WriteLine(
                "By using this software you agree to the terms in \"license.txt\".");

            Cfg.Reload();

            if (string.IsNullOrEmpty(Cfg.config.apikey))
            {
                Console.WriteLine("Fatal error: API key is missing. Please fill in the API key field in \"config.cfg\"");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            _apiKey = Cfg.config.apikey;

            if (string.IsNullOrEmpty(Cfg.config.username) || string.IsNullOrEmpty(Cfg.config.password))
            {
                Console.WriteLine("Please input your username and password.");

                Console.Write("Username: ");
                _user = Console.ReadLine();

                Console.Write("Password: ");
                _pass = Console.ReadLine();
            }
            else
            {
                _user = Cfg.config.username;
                _pass = Cfg.config.password;
            }

            Console.WriteLine("Attempting web login...");

            _account = Web.DoLogin(_user, _pass, Cfg.config.steamMachineAuth);

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

            while (isPolling) //permanent loop, can be changed 
            {
                var recData = new Dictionary<string, string>
                {
                    {"get_received_offers", "1"},
                    {"active_only", "1"},
                    {"time_historical_cutoff", "999999999999"}
                    //arbitrarily high number to retrieve latest offers
                };

                var offers = offerHandler.GetTradeOffers(recData).trade_offers_received;

                if (offers == null) continue;

                foreach (CEconTradeOffer cEconTradeOffer in offers)
                {
                    if (cEconTradeOffer.items_to_give == null)
                    {
                        offerHandler.AcceptTradeOffer(Convert.ToUInt64(cEconTradeOffer.tradeofferid),
                            _account.AuthContainer,
                            cEconTradeOffer.accountid_other, "1");
                        Console.WriteLine("Accepted a donation!");
                    }
                    else
                    {
                        offerHandler.DeclineTradeOffer(Convert.ToUInt64(cEconTradeOffer.tradeofferid));
                        Console.WriteLine("Refused a \"donation\" that would have taken items from us.");
                    }
                }
                Thread.Sleep(10000);
            }
        }
    }
}
