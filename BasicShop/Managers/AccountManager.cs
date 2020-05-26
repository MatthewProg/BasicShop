using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Managers
{
    public class AccountManager
    {
        public static int? LoggedId { get; private set; }

        public static bool Login(string username, string password)
        {
            bool output = false;

            try
            {
                var a = GenerateHashSalt("OK");
                var b = GenerateHashSalt("OK");
                var test = a == b;
                var test2 = ChecklHashSalt("OK", a);
                var test3 = ChecklHashSalt("OK", b);
            }
            catch(Exception e)
            {
                string mess = "Podczas logowania wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }

        private static string GenerateHashSalt(string password)
        {
            var saltBytes = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            return salt + hashPassword;
        }

        private static bool ChecklHashSalt(string password, string hs)
        {
            var salt = hs.Substring(0, 88);
            var hash = hs.Substring(88);

            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return (hashPassword == hash);
        }
    }
}
