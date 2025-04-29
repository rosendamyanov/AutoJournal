namespace AutoJournal.Services.Validation.AuthValidation.Contracts
{
    public interface IPasswordValidation
    {
        bool IsStrong(string password);
    }
}