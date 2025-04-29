using AutoJournal.Common.Response;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> Register(UserRegisterRequestDTO user);
        Task<ApiResponse<AuthResponse>> Login(UserLoginRequestDTO requestUser);
    }
}