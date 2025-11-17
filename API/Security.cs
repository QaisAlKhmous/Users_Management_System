using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Security
    {
        public static string HashPassword(string password)
        {

            var salt = Guid.NewGuid().ToString();
            var combined = password + salt;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
                string hash = Convert.ToBase64String(bytes);

                return $"{hash}:{salt}";
            }
        }


        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                return false;

            var hash = parts[0];
            var salt = parts[1];

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword + salt));
                string enteredHash = Convert.ToBase64String(bytes);

                return enteredHash == hash;
            }
        }
    }

}
