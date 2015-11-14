using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTradeOffers;
using NUnit.Framework;

namespace CSharpTradeOffers_Tests
{
    [TestFixture]
    class IdConversions_Tests
    {
        [Test]
        public void AccountIdToSteamIdText_NoFail()
        {
            uint input = 100049908;
            string expected = "STEAM_0:0:50024954";
            string actual = IdConversions.AccountIdToSteamIdText(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AccountIdToUlong_NoFail()
        {
            uint input = 100049908;
            ulong expected = 76561198060315636;
            ulong actual = IdConversions.AccountIdToUlong(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SteamIdTextToAccountId_NoFail()
        {
            string input = "STEAM_0:0:50024954";
            uint expected = 100049908;
            uint actual = IdConversions.SteamIdTextToAccountId(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SteamIdTextToUlong_NoFail()
        {
            string input = "STEAM_0:0:50024954";
            ulong expected = 76561198060315636;
            ulong actual = IdConversions.SteamIdTextToUlong(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UlongToAccountId_NoFail()
        {
            ulong input = 76561198060315636;
            uint expected = 100049908;
            uint actual = IdConversions.UlongToAccountId(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UlongToSteamIdText_NoFail()
        {
            ulong input = 76561198060315636;
            string expected = "STEAM_0:0:50024954";
            string actual = IdConversions.UlongToSteamIdText(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
