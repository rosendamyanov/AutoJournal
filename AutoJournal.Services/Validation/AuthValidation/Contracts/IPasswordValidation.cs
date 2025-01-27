namespace AutoJournal.Services.Validation.AuthValidation.Contracts
{
    public interface IPasswordValidation
    {
        bool IsEmailValid(string email);
    }
}