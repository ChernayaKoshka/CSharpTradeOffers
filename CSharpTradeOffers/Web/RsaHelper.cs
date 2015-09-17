using System;
using System.Security.Cryptography;
using System.Text;
using CSharpTradeOffers.Json;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Web
{
    internal class RsaHelper
    {
        private readonly string _password;

        public RsaHelper(string password)
        {
            _password = password;
        }

        public string EncryptPasswordResponse(IResponse response)
        {
            if (!RequestRsaKey(response.ReadStream())) return null;

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

        private bool RequestRsaKey(string response)
        {
            RsaKey rsaJson = JsonConvert.DeserializeObject<RsaKey>(response);
            if (!rsaJson.Success) return false;
            RsaJson = rsaJson;
            return true;
        }


        private static byte[] HexToByte(string hex)
        {
            if (hex.Length%2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];
            int l = hex.Length;

            for (int i = 0; i < (l >> 1); ++i)
            {
                arr[i] = (byte) ((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : 55);
        }

        public RsaKey RsaJson { get; private set; }
    }
}