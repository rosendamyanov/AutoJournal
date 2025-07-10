using AutoJournal.Authentication.Validation.Interfaces;
using System.Text.RegularExpressions;

namespace AutoJournal.Authentication.Validation
{
    public class PasswordValidation : IPasswordValidation
    {
        public bool IsStrong(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,64}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
