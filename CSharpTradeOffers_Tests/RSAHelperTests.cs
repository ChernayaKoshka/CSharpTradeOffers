using System;
using CSharpTradeOffers.Web;
using Moq;
using NUnit.Framework;

namespace CSharpTradeOffers_Tests
{
    [TestFixture]
    class RsaHelperTests
    {
        private const string TestRsaJsonStringFalse = "{\"success\":false}";

        private const string TestRsaJsonStringOdd = "{\"success\":true," +
                                                    "\"publickey_mod\":\"A53\"," +
                                                    "\"publickey_exp\":\"010001\"," +
                                                    "\"timestamp\":\"108088400000\"," +
                                                    "\"steamid\":\"00000000000000000\"," +
                                                    "\"token_gid\":\"00000000000\"}";

        private const string TestPassword =  "DummyPass";
        private readonly RsaHelper _helper = new RsaHelper(TestPassword);

        private Mock<IResponse> _mockResponseFalse;
        private Mock<IResponse> _mockResponseOdd;

        [SetUp]
        public void BeforeEachTest()
        {
            /*
            {"success":false}
            */
            _mockResponseFalse = new Mock<IResponse>();
            _mockResponseFalse.Setup(x => x.ReadStream()).Returns(TestRsaJsonStringFalse);

            _mockResponseOdd = new Mock<IResponse>();
            _mockResponseOdd.Setup(x => x.ReadStream()).Returns(TestRsaJsonStringOdd);
        }

        [Test]
        public void EncryptPassword_ReturnsNull_WhenJsonSuccessIsFalse()
        {
            string actual = _helper.EncryptPassword(_mockResponseFalse.Object);
            Assert.IsNull(actual);
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public void EncryptPassword_ThrowsException_WhenBinaryKeyIsOdd()
        {
            _helper.EncryptPassword(_mockResponseOdd.Object);
        }
    }
}
