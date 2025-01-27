using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Contracts;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;
using AutoJournal.Services.Services.Contracts;

namespace AutoJournal.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IAuthFactory _userFactory;
        public AuthService(IAuthRepository userRepository, IAuthFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<bool> Register(UserRegisterRequestDTO requestUser)
        {
            //Should implement user validations(username uniqueness, email uniqueness, password chars etc)

            //Username and Email 
            if(await _userRepository.UserExists(requestUser.Username, requestUser.Email))
            {
                return false;
            }


            User user = _userFactory.Map(requestUser);

            bool response = await _userRepository.Register(user);

            if (response)
            {
                return true;
            }
            return false;
        }
    }
}
