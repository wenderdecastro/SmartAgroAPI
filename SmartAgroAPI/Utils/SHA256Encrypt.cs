using System.Security.Cryptography;
using System.Text;

namespace SmartAgroAPI.Utils
{
    public static class SHA256Encrypt
    {
        public static string HashPassword(string input)
        {
            var inputByte = Encoding.UTF8.GetBytes(input);
            var hash = SHA256.HashData(inputByte);
            return Encoding.UTF8.GetString(hash);
        }

        public static bool CompareHash(string input, string encrypted)
        {
            return HashPassword(input) == encrypted;
        }



    }
}
