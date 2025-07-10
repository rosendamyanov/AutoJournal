namespace AutoJournal.Authentication.Validation.Interfaces
{
    public interface IPhoneNumberValidation
    {
        string IsValidPhoneNumber(string phoneNumber, string countryCode);
    }
}