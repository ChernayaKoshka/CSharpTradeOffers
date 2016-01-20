using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CSharpTradeOffers;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Trading;
using CSharpTradeOffers.Web;

namespace ExchangeBot
{
    internal class Program
    {
        private static Account _account;
        private static DefaultConfig _config = new DefaultConfig();
        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        private static void Main()
        {
            Console.Title = "Exchange Bot by sne4kyFox";
            Console.WriteLine("Welcome to the exchange bot!");
            Console.WriteLine(
                "By using this software you agree to the terms in \"license.txt\".");

            _config = ConfigHandler.Reload();

            ConfigErrorResult errors = _config.CheckForErrors();
            if (!errors.Valid)
            {
                Console.WriteLine(errors.ErrorMessage);
                Console.ReadLine();
                Environment.Exit(-1);
            }

            Console.WriteLine("Attempting web login...");

            _account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, _config.Username, _config.Password, _config.SteamMachineAuth);

            if (!string.IsNullOrEmpty(_account.SteamMachineAuth))
            {
                _config.SteamMachineAuth = _account.SteamMachineAuth;
                ConfigHandler.WriteChanges(_config);
            }

            Console.WriteLine("Login was successful!");

            PollOffers();
        }

        private static void PollOffers()
        {
            Console.WriteLine("Polling offers every ten seconds.");

            bool isPolling = true;

            var offerHandler = new EconServiceHandler(_config.ApiKey);
            var marketHandler = new MarketHandler();

            Inventory csgoInventory = new Inventory(_account.SteamId, 730);

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
                    TradeOffer offer = new TradeOffer();
                    offer.Them.Assets = cEconTradeOffer.ItemsToReceive;
                    offer.Me.Assets.Add(csgoInventory.Items.First().Value.Items.First().ToCEconAsset(730));
                    offerHandler.ModifyTradeOffer(IdConversions.AccountIdToUlong(cEconTradeOffer.AccountIdOther),
                        "Here you go!", "1", cEconTradeOffer.TradeOfferId, offer, _account.AuthContainer);
                }
            }
        }
    }
}
