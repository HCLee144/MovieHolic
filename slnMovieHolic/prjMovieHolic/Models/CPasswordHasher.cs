using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace prjMovieHolic.Models
{
    public class CPasswordHasher
    {//將新密碼加密
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using PBKDF2
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Combine the salt and hash into a single string
            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);
            return Convert.ToBase64String(hashBytes);
        }
        //驗證加密後的密碼
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and hash from the hashed password
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[128 / 8];
            Array.Copy(hashBytes, 0, salt, 0, salt.Length);
            byte[] hash = new byte[hashBytes.Length - salt.Length];
            Array.Copy(hashBytes, salt.Length, hash, 0, hash.Length);

            // Hash the password with the extracted salt
            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Compare the computed hash with the extracted hash
            return computedHash.SequenceEqual(hash);
        }
    }
}
