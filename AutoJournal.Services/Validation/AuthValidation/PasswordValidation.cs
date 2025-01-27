using AutoJournal.Services.Validation.AuthValidation.Contracts;
using System.Text.RegularExpressions;

namespace AutoJournal.Services.Validation.AuthValidation
{
    public class PasswordValidation : IPasswordValidation
    {
        public bool IsEmailValid(string email)
        {
            if(string.IsNullOrEmpty(email)) return false;

            string regexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, regexPattern);
        }
    }
}
