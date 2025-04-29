using AutoJournal.Common.Response;
using AutoJournal.Services.Validation.AuthValidation.Contracts;
using System.Text.RegularExpressions;

namespace AutoJournal.Services.Validation.AuthValidation
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
