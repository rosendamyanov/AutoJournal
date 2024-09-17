using AutoJournal.DTOs.Request;

namespace AutoJournal.Services.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> Register(UserRegisterRequestDTO user);
    }
}