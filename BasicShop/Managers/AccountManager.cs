using MahApps.Metro.Controls.Dialogs;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
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

            if (username == null || username == string.Empty || password == null || password == string.Empty)
                return false;

            string hashSalt = string.Empty;
            try
            {
                using (var dataContext = new shopEntities())
                    hashSalt = dataContext.account.Where(x => x.username == username).Select(x => x.password).FirstOrDefault();
            }
            catch (Exception e)
            {
                string mess = "Podczas logowania wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            if (hashSalt == null) return false;
            if (!ChecklHashSalt(password, hashSalt)) output = false;
            else output = true;

            if (output)
            {
                try
                {
                    LoggedId = new shopEntities().account.Where(x => x.username == username).Select(x => x.account_id).FirstOrDefault();
                }
                catch (Exception e)
                {
                    string mess = "Podczas logowania wystąpił błąd!\n";
                    StandardMessages.Error(mess + e.Message);
                }
            }
            return output;
        }

        public static bool Register(string username, string email, string password, string firstname, string lastname, out string message)
        {
            try
            {
                var dataContext = new shopEntities();
                /*if(dataContext.account.Where(x=>x.username == username).Any())
                {
                    message = "Użytkownik z taką nazwą już istnieje";
                    return false;
                }

                if (dataContext.account.Where(x => x.email == email).Any())
                {
                    message = "Użytkownik z takim emailem już istnieje";
                    return false;
                }*/

                account a = new account();
                a.email = email;
                a.username = username;
                a.password = GenerateHashSalt(password);
                a.role_id = 1;
                a.person = new person();
                a.person.firstname = firstname;
                a.person.surname = lastname;

                dataContext.account.Add(a);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Podczas rejestracji wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
                message = "Wystąpił błąd";
                return false;
            }

            message = "Zarejestrowano pomyślnie!";
            return true;
        }

        public static void Logout()
        {
            LoggedId = null;
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
