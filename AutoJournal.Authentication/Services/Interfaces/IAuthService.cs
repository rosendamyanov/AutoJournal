using AutoJournal.Common.Response;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;

namespace AutoJournal.Authentication.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> Register(UserRegisterRequestDTO user);
        Task<ApiResponse<AuthResponseDto>> Login(UserLoginRequestDTO requestUser);
    }
}