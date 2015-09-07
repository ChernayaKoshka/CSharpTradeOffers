using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpTradeOffers;

namespace CSharpTradeOffers_Tests
{
    [TestClass]
    public class SteamIDOperations_Tests
    {
        [TestMethod]
        public void ConvertAccountIdtoUInt64_NoFail()
        {
            uint input = 100049908;
            ulong expected = 76561198060315636;
            ulong actual = SteamIdOperations.ConvertAccountIdToUlong(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertAccountIdToSteamId_NoFail()
        {
            uint input = 100049908;
            string expected = "STEAM_0:0:50024954";
            string actual = SteamIdOperations.ConvertAccountIdToSteamId(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSteamIdtoULong_NoFail()
        {
            string input = "STEAM_0:0:50024954";
            ulong expected = 76561198060315636;
            ulong actual = SteamIdOperations.ConvertSteamIdToUlong(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ConvertSteamIdtoUlong_Exception_IndexOutOfRangeException()
        {
            ulong expected = 76561198060315636;
            string input = "STEAM_00:50024954";
            ulong actual = SteamIdOperations.ConvertSteamIdToUlong(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertUlongToSteamId_NoFail()
        {
            ulong input = 76561198060315636;
            string expected = "STEAM_0:0:50024954";
            string actual = SteamIdOperations.ConvertUlongToSteamId(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSteamIdToAccountId_NoFail()
        {
            string input = "STEAM_0:0:50024954"; 
            uint expected = 100049908;
            uint actual = SteamIdOperations.ConvertSteamIdToAccountId(input);
            Assert.AreEqual(expected, actual);
        }
        //100049904
    }
}
