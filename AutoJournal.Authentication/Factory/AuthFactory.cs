using AutoJournal.Data.Models;
using AutoJournal.DTOs.Request;
using AutoJournal.Authentication.Factory.Interfaces;

namespace AutoJournal.Authentication.Factory
{
    public class AuthFactory : IAuthFactory
    {
        public User Map(UserRegisterRequestDTO request)
        {
            User user = new User()
            {
                Id = new Guid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password,
                PhoneNumber = request.PhoneNumber,
                FullPhoneNumber = "359882223626",
                PhoneCountryCode = "BG",
                RefreshTokens = new List<RefreshToken>()
            };
            return user;
        }
    }
}
