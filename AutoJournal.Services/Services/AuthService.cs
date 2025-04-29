using AutoJournal.Data.Models;
using AutoJournal.Common.Response;
using AutoJournal.Data.Repositories.Contracts;
using AutoJournal.DTOs.Request;
using AutoJournal.Services.Factory.Contracts;
using AutoJournal.Services.Services.Contracts;
using AutoJournal.Services.Validation.AuthValidation.Contracts;
using BCrypt.Net;
using AutoJournal.DTOs.Response;
using AutoJournal.Common.Models;
using Microsoft.Extensions.Options;

namespace AutoJournal.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IAuthFactory _authFactory;
        private readonly IEmailValidation _emailValidation;
        private readonly IPasswordValidation _passwordValidation;
        private readonly IJwtService _jwtService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public AuthService(
            IAuthRepository userRepository, 
            IAuthFactory userFactory, 
            IEmailValidation emailValidation, 
            IPasswordValidation passwordValidation, 
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _authFactory = userFactory;
            _emailValidation = emailValidation;
            _passwordValidation = passwordValidation;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings;
            
        }

        public async Task<ApiResponse<AuthResponse>> Register(UserRegisterRequestDTO requestUser)
        {
            //TO DO:
            //3.Phone number validation.
            //4.Validation if the email domain is reachable - SignalR
            //5.Logging Erros in .log files or app console for easier debugging and monitorig
            

            
            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<AuthResponse>.Failure(ResponseMessages.InvalidEmail, ResponseCodes.InvalidEmail);
            }

            if (!_passwordValidation.IsStrong(requestUser.Password))
            {
                return ApiResponse<AuthResponse>.Failure(ResponseMessages.WeakPassword, ResponseCodes.WeakPassword);
            }
            
            //Username and Email check if they already exist.
            (bool usernameExists, bool emailExists) = await _userRepository.CheckUserExistenceAsync(requestUser.Username, requestUser.Email);

            switch(true)
            {
                case true when usernameExists:
                    return ApiResponse<AuthResponse>.Failure(ResponseMessages.UsernameExists, ResponseCodes.UsernameExists);
                case true when emailExists:
                    return ApiResponse<AuthResponse>.Failure(ResponseMessages.EmailExists, ResponseCodes.EmailExists);
                case true when usernameExists  && emailExists:
                    return ApiResponse<AuthResponse>.Failure(ResponseMessages.UsernameAndEmailExists, ResponseCodes.UsernameAndEmailExists);
            }

                
            requestUser.Password = BCrypt.Net.BCrypt.HashPassword(requestUser.Password);
                 
            User user = _authFactory.Map(requestUser);

            bool registrationSuccess = await _userRepository.Register(user);

            if (!registrationSuccess)
            {
                return ApiResponse<AuthResponse>.Failure(ResponseMessages.RegistratoinFailed, ResponseCodes.RegistrationFailed);
            }

            //Generate tokens
            var accessToken = _jwtService.GenerateAccessToken(user);
            var (rawRefreshToken, refreshToken) = _jwtService.GenerateRefreshToken();
            //Store tokens
            refreshToken.UserId = user.Id;
            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            return ApiResponse<AuthResponse>.Success(new AuthResponse()
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenExpirationMinutes)
            },ResponseMessages.UserRegistered);

        }

        public async Task<ApiResponse<AuthResponse>> Login(UserLoginRequestDTO requestUser)
        {
            User? user = await _userRepository.GetUserByIdentifier(requestUser.Username);

            if (user == null)
            {
                return ApiResponse<AuthResponse>.Failure(ResponseMessages.UserNotFound, ResponseCodes.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(requestUser.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponse>.Failure(ResponseMessages.InvalidCredentials, ResponseCodes.InvalidCredentials);
            }

            // Generate tokens
            var accessToken = _jwtService.GenerateAccessToken(user);
            var (rawRefreshToken, refreshToken) = _jwtService.GenerateRefreshToken();

            // Store refresh token
            refreshToken.UserId = user.Id;
            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            return ApiResponse<AuthResponse>.Success(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenExpirationMinutes)
            }, ResponseMessages.UserLogged);
        }
    }
}
