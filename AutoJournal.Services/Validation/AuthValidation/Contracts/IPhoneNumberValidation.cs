namespace AutoJournal.Services.Validation.AuthValidation.Contracts
{
    public interface IPhoneNumberValidation
    {
        string IsValidPhoneNumber(string phoneNumber, string countryCode);
    }
}