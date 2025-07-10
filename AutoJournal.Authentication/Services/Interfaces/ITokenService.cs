using AutoJournal.Data.Models;
using System.Security.Claims;

namespace AutoJournal.Authentication.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string RawToken, RefreshToken HashedToken) GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
        bool ValidateRefreshToken(string rawToken, RefreshToken storedToken);
    }
}