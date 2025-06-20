using AutoJournal.Common.Response;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> Register(UserRegisterRequestDTO user);
        Task<ApiResponse<AuthResponseDto>> Login(UserLoginRequestDTO requestUser);
    }
}