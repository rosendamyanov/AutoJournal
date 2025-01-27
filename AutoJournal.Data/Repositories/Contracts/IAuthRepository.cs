using AutoJournal.Data.Models;

namespace AutoJournal.Data.Repositories.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> Register(User user);
        Task<bool> UserExists(string username, string email);
    }
}