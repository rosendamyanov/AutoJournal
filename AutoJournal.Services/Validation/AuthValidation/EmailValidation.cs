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

            string pattern = @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
