using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class HashingBL
    {
        public static (byte[], byte[]) HashPassword(string password, byte[] salt)
        {
            using (var sha512 = new SHA512Managed())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var combinedBytes = passwordBytes.Concat(salt).ToArray();
                var hashBytes = sha512.ComputeHash(combinedBytes);

                return (hashBytes, salt);
            }
        }

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltbytes = new byte[16];
                rng.GetBytes(saltbytes);
                return saltbytes;
            }
        }
    }
}
