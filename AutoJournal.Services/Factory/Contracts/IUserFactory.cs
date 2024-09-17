using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;

namespace AutoJournal.Services.Factory.Contracts
{
    public interface IUserFactory
    {
        User Map(UserRegisterRequestDTO request);
    }
}