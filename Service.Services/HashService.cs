using Service.Common.Interfaces.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace Service.Services
{
    internal class HashService : IHashService
    {
        public (byte[] hash, byte[] key) GetHash(string value, byte[]? key = null)
        {
            using HMACSHA512 hmac = key != null? new HMACSHA512(key) : new HMACSHA512();
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));

            return (hash, hmac.Key);
        }
    }
}