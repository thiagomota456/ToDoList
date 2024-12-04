using System.Security.Cryptography;
using System.Text;

namespace ToDoListAPI.Services
{
    public static class Cryptography
    {
        public static string GenerateHash(string value)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool ValidateHash(string value, string hash)
        {
            var valueHash = GenerateHash(value);
            return valueHash == hash;
        }
    }
}
