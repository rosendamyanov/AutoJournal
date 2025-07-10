using AutoJournal.Authentication.Validation.Interfaces;
using System.Text.RegularExpressions;


namespace AutoJournal.Authentication.Validation
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
