using AutoJournal.Common.Response;
using AutoJournal.DTOs.Request;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Register(UserRegisterRequestDTO user);
        Task<ApiResponse<string>> Login(UserLoginRequestDTO requestUser);
    }
}