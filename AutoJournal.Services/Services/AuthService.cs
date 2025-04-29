using AutoJournal.Data.Models;
using AutoJournal.Common.Response;
using AutoJournal.Data.Repositories.Contracts;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;
using AutoJournal.Services.Services.Contracts;
using AutoJournal.Services.Validation.AuthValidation.Contracts;
using BCrypt.Net;

namespace AutoJournal.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IAuthFactory _authFactory;
        private readonly IEmailValidation _emailValidation;
        private readonly IPasswordValidation _passwordValidation;
        public AuthService(IAuthRepository userRepository, IAuthFactory userFactory, IEmailValidation emailValidation, IPasswordValidation passwordValidation)
        {
            _userRepository = userRepository;
            _authFactory = userFactory;
            _emailValidation = emailValidation;
            _passwordValidation = passwordValidation;
        }

        public async Task<ApiResponse<string>> Register(UserRegisterRequestDTO requestUser)
        {
            //TO DO:
            //1.Add JWT.
            //3.Phone number validation.
            //4.Validation if the email domain is reachable - SignalR
            //5.Logging Erros in .log files or app console for easier debugging and monitorig
            

            
            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<string>.Failure(ResponseMessages.InvalidEmail, ResponseCodes.InvalidEmail);
            }

            if (!_passwordValidation.IsStrong(requestUser.Password))
            {
                return ApiResponse<string>.Failure(ResponseMessages.WeakPassword, ResponseCodes.WeakPassword);
            }
            
            //Username and Email check if they already exist.
            (bool usernameExists, bool emailExists) = await _userRepository.CheckUserExistenceAsync(requestUser.Username, requestUser.Email);

            switch(true)
            {
                case true when usernameExists:
                    return ApiResponse<string>.Failure(ResponseMessages.UsernameExists, ResponseCodes.UsernameExists);
                case true when emailExists:
                    return ApiResponse<string>.Failure(ResponseMessages.EmailExists, ResponseCodes.EmailExists);
                case true when usernameExists  && emailExists:
                    return ApiResponse<string>.Failure(ResponseMessages.UsernameAndEmailExists, ResponseCodes.UsernameAndEmailExists);
            }

                
            requestUser.Password = BCrypt.Net.BCrypt.HashPassword(requestUser.Password);
                 
            User user = _authFactory.Map(requestUser);

            bool response = await _userRepository.Register(user);

            return response
                ? ApiResponse<string>.Success(ResponseMessages.UserRegistered)
                : ApiResponse<string>.Failure(ResponseMessages.RegistratoinFailed, ResponseCodes.RegistrationFailed);
        }

        public async Task<ApiResponse<string>> Login(UserLoginRequestDTO requestUser)
        {
            User? user = await _userRepository.GetUserByIdentifier(requestUser.Username);

            if (user == null)
            {
                return ApiResponse<string>.Failure(ResponseMessages.UserNotFound, ResponseCodes.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(requestUser.Password, user.PasswordHash))
            {
                return ApiResponse<string>.Failure(ResponseMessages.InvalidCredentials, ResponseCodes.InvalidCredentials);
            }

            return ApiResponse<string>.Success(ResponseMessages.UserLogged);
        }
    }
}
