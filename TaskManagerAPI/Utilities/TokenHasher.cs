using System.Security.Cryptography;
using System.Text;

namespace TaskManagerAPI.Utilities
{
    public static class TokenHasher
    {
        public static string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes);
        }
    }
}
