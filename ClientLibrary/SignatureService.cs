using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Latoken_CSharp_Client_Library
{
    public static class SignatureService
    {
        public static string CreateSignature(string key, string message, string signatureType = "HMAC-SHA256")
        {
            switch (signatureType.ToUpper())
            {
                case "HMAC-SHA256":
                    {
                        return HashHmac256(key, message);
                    }
                case "HMAC-SHA512":
                    {
                        return HashHmac512(key, message);
                    }
                case "HMAC-SHA384":
                    {
                        return HashHmac384(key, message);
                    }
                default:
                    return string.Empty;
            }

        }

        #region Hash Hex Functions

        private static string HashHmac256(string key, string message)
        {
            byte[] hash = HashHmac(StringEncode(key), StringEncode(message));
            return HashEncode(hash);
        }

        private static string HashHmac512(string key, string message)
        {
            byte[] hash = HashHmac512(StringEncode(key), StringEncode(message));
            return HashEncode(hash);
        }

        private static string HashHmac384(string key, string message)
        {
            byte[] hash = HashHmac384(StringEncode(key), StringEncode(message));
            return HashEncode(hash);
        }

        #endregion

        #region Hash Functions
        private static byte[] HashHmac(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        private static byte[] HashHmac512(byte[] key, byte[] message)
        {
            var hash = new HMACSHA512(key);
            return hash.ComputeHash(message);
        }

        private static byte[] HashHmac384(byte[] key, byte[] message)
        {
            var hash = new HMACSHA384(key);
            return hash.ComputeHash(message);
        }
        #endregion

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}
