using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;

namespace AutoJournal.Services.Factory
{
    public class UserFactory : IUserFactory
    {
        public User Map(UserRegisterRequestDTO request)
        {
            User user = new User()
            {
                Id = new Guid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                PhoneNumber = request.PhoneNumber,
            };
            return user;
        }
    }
}
