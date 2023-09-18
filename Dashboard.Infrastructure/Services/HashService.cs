using Dashboard.Application.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.Infrastructure.Services
{
    public class HashService : IHashService
    {
        public string GetHash(string key)
        {
            var sha256 = new SHA256Managed();
            var bytes = Encoding.UTF8.GetBytes(key);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
