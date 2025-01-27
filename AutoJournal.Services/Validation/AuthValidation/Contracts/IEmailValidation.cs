namespace AutoJournal.Services.Validation.AuthValidation.Contracts
{
    public interface IEmailValidation
    {
        bool IsEmailValid(string email);
    }
}