using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PeterKrausAP05LAP.Models
{
    public class SecureManager
    {
        public static (string salt, string hash) GenerateHash(string password, string usersalt = "")
        {
            byte[] salt = new byte[128 / 8];

            if (usersalt == "")
            {
                // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
            }
            else
            {
                for (int i = 0; i < salt.Length; i++)
                {
                    salt[i] = (byte)usersalt[i];
                }
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            string salttest = "";

            for (int i = 0; i < salt.Length; i++)
            {
                salttest += (char)salt[i];
            }


            return (salttest, hashed);
        }

    }
}