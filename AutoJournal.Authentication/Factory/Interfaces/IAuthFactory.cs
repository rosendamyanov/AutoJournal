using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;

namespace AutoJournal.Authentication.Factory.Interfaces
{
    public interface IAuthFactory
    {
        User Map(UserRegisterRequestDTO request);
        RevokedToken Map(RefreshToken refreshToken);
        AuthResponseDto Map(string accessToken, string rawRefreshToken, Guid refreshTokenId, DateTime expiryDate);

    }
}