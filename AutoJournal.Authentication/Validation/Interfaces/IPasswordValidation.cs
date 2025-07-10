namespace AutoJournal.Authentication.Validation.Interfaces
{
    public interface IPasswordValidation
    {
        bool IsStrong(string password);
    }
}