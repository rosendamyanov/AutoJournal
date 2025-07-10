using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;

namespace AutoJournal.Authentication.Factory.Interfaces
{
    public interface IAuthFactory
    {
        User Map(UserRegisterRequestDTO request);
    }
}