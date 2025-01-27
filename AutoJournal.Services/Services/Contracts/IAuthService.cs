using AutoJournal.DTOs.Request;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterRequestDTO user);
    }
}