using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VehicleTracking.Core
{
    public class CryptoHelper
    {
        public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(salt);
            }

            var rfc = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = rfc.GetBytes(20);

            byte[] hashedBytes = new byte[36];
            Array.Copy(salt, 0, hashedBytes, 0, 16);
            Array.Copy(hash, 0, hashedBytes, 16, 20);
            return Convert.ToBase64String(hashedBytes);
        }
        public bool CheckPassword(string savedHash, string inputedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(savedHash);
            byte[] salt = new byte[128 / 8];

            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(inputedPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
                else
                    return true;
            }

            return false;
        }
    }
}
