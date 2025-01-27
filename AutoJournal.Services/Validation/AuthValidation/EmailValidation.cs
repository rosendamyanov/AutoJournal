using AutoJournal.Common.Response;
using AutoJournal.Services.Validation.AuthValidation.Contracts;
using System.Text.RegularExpressions;


namespace AutoJournal.Services.Validation.AuthValidation
{
    public class EmailValidation :IEmailValidation
    {
        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
