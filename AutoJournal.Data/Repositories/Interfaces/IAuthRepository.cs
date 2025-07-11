using AutoJournal.Data.Models;

namespace AutoJournal.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<(bool usernameExists, bool emailExists)> CheckUserExistenceAsync(string username, string email);
        Task<User?> GetUserByIdentifier(string identifier);
        Task<User?> GetUserRefreshToken(string username);
        Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId, Guid userId);
        Task<bool> SaveRevokedRefreshTokenAsync(RevokedToken revokedToken, RefreshToken refreshToken);
        Task<bool> SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> SaveUserRefreshTokenAsync(User user);
    }
}