using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Contracts;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;
using AutoJournal.Services.Services.Contracts;

namespace AutoJournal.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        public UserService(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<bool> Register(UserRegisterRequestDTO requestUser)
        {
            //Should implement user validations(username uniqueness, email uniqueness, password chars etc)

            
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
