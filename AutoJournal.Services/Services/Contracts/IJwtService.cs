using AutoJournal.Data.Models;
using System.Security.Claims;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        (string RawToken, RefreshToken HashedToken) GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
        bool ValidateRefreshToken(string rawToken, RefreshToken storedToken);
    }
}