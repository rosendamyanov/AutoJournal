using AutoJournal.Data.Models;

namespace AutoJournal.Data.Repositories.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<(bool usernameExists, bool emailExists)> CheckUserExistenceAsync(string username, string email);
        Task<User?> GetUserByIdentifier(string identifier);
        Task SaveRefreshTokenAsync(RefreshToken newToken);
    }
}