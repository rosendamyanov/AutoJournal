using AutoJournal.Data.Models;
using AutoJournal.Common.Response;
using AutoJournal.Data.Repositories.Contracts;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;
using AutoJournal.Services.Services.Contracts;
using AutoJournal.Services.Validation.AuthValidation.Contracts;

namespace AutoJournal.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IAuthFactory _authFactory;
        private readonly IEmailValidation _emailValidation;
        public AuthService(IAuthRepository userRepository, IAuthFactory userFactory, IEmailValidation emailValidation)
        {
            _userRepository = userRepository;
            _authFactory = userFactory;
            _emailValidation = emailValidation;
        }

        public async Task<ApiResponse<string>> Register(UserRegisterRequestDTO requestUser)
        {
            //Should implement user validations(username uniqueness, email uniqueness, password chars etc)

            //Username and Email 
            if(await _userRepository.UserExists(requestUser.Username, requestUser.Email))
            {
                return ApiResponse<string>.Failure("Username or email already exists.", "USER_EXISTS");
            }

            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<string>.Failure("Email is not valid", "EMAIL_INVALID");
            }

            User user = _authFactory.Map(requestUser);

            bool response = await _userRepository.Register(user);

            if (response)
            {
                return ApiResponse<string>.Success("User registered successfully.");
            }
            return ApiResponse<string>.Failure("User registration failed, please try again", "REGISTRATION_FAILED");
        }
    }
}
