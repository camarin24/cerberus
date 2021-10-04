using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Intecgra.Cerberus.Domain.Utilities
{
    public static class PasswordHash
    {
        public static string Create(string value, string salt = "")
        {
            if (string.IsNullOrEmpty(salt)) salt = CreateSalt();

            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool Validate(string password, string hashedPassword, string salt)
        {
            return Create(password, salt) == hashedPassword;
        }


        public static string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}