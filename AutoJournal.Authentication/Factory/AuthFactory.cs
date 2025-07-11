using AutoJournal.Authentication.Configuration;
using AutoJournal.Authentication.Factory.Interfaces;
using AutoJournal.Common.Messages;
using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;

namespace AutoJournal.Authentication.Factory
{
    public class AuthFactory : IAuthFactory
    {
        public User Map(UserRegisterRequestDTO request)
        {
            User user = new User()
            {
                Id = new Guid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password,
                PhoneNumber = request.PhoneNumber,
                FullPhoneNumber = "359882223626",
                PhoneCountryCode = "BG",
                RefreshTokens = new List<RefreshToken>()
            };
            return user;
        }

        public RevokedToken Map(RefreshToken refreshToken)
        {
            RevokedToken revokedToken = new RevokedToken()
            {
                Id = new Guid(),
                TokenHash = refreshToken.TokenHash,
                RevokedAt = DateTime.UtcNow,
                Reason = Messages.tokenRefreshed,
                UserId = refreshToken.UserId,
                User = refreshToken.User,
            };
            return revokedToken;
        }

        public AuthResponseDto Map(string accessToken, string rawRefreshToken, Guid refreshTokenId, DateTime expiryDate)
        {
            AuthResponseDto tokens = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                RefreshTokenId = refreshTokenId,
                AccessTokenExpiry = expiryDate
            };
            return tokens;
        }
    }
}
