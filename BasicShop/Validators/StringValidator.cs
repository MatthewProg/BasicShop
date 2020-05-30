using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BasicShop.Validators
{
    public class StringValidator
    {
        public static bool ValidateEmail(string email)
        {
            string simplePattern = @"^\b[\w\.-]+@[\w\.-]+\.\w{2,}\b$";
            if (!Regex.IsMatch(email, simplePattern)) return false;
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool ValidatePhoneNumber(string phone)
        {
            string pattern = @"^(\d{9})$|^([+]\d{11})$";
            return Regex.IsMatch(phone, pattern);
        }
        public static bool ValidateName(string name)
        {
            string pattern = @"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$";
            return Regex.IsMatch(name, pattern);
        }
    }
}
