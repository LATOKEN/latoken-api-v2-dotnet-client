using System;
using System.Security.Cryptography;
using System.Text;

namespace Latoken.Api.Client.Library
{
    public static class SignatureService
    {
        public static string CreateSignature(string key, string message)
        {
            return HashHmac256(key, message);
        }

        private static string HashHmac256(string key, string message)
        {
            var hash = new HMACSHA256(StringEncode(key));
            return HashEncode(hash.ComputeHash(StringEncode(message)));
        }

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