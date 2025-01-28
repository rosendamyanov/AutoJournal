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
            //TO DO:
            //1.Validation if the email domain is reachable - SignalR
            //2.Password encryption
            //3.Validation for specific/needed password chars "!@#$%^&*", uppercase and lowercase letters is a must, 8-64 chars!
            //4.Phone number validation.

            //Username and Email 
            if (await _userRepository.UserExists(requestUser.Username, requestUser.Email))
            {
                return ApiResponse<string>.Failure(ResponseMessages.UserExists, ResponseCodes.UserExists);
            }

            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<string>.Failure(ResponseMessages.InvalidEmail, ResponseCodes.InvalidEmail);
            }

            User user = _authFactory.Map(requestUser);

            bool response = await _userRepository.Register(user);

            return response
                ? ApiResponse<string>.Success(ResponseMessages.UserRegistered)
                : ApiResponse<string>.Failure(ResponseMessages.RegistratoinFailed, ResponseCodes.RegistrationFailed);
        }

        public async Task<ApiResponse<string>> Login(UserLoginRequestDTO requestUser)
        {
            User user = await _userRepository.GetUserByIdentifier(requestUser.Username);

            if (user == null)
            {
                return ApiResponse<string>.Failure("User not found", "USER_NOT_FOUND");
            }

            if(requestUser.Password != user.PasswordHash)
            {
                return ApiResponse<string>.Failure("Wrong username or password", "INVALID_CREDENTIALS");
            }

            return ApiResponse<string>.Success("Successfully logged in.");
        }
    }
}
