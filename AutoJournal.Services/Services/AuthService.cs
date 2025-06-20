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
        private readonly ITokenService _tokenService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public AuthService(
            IAuthRepository userRepository, 
            IAuthFactory userFactory, 
            IEmailValidation emailValidation, 
            IPasswordValidation passwordValidation, 
            ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _authFactory = userFactory;
            _emailValidation = emailValidation;
            _passwordValidation = passwordValidation;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
            
        }

        //TO DO:
        //1. Token Refresh (POST /auth/refresh)
        //2. Logout/Token Revocation (POST /auth/logout)
        //3. Password Reset Flow (request + confirm endpoints)
        //4. Email Verification (POST /auth/verify-email)
        //5. Session Management (list/revoke sessions)
        //6. Two-Factor Authentication (enable/verify)
        //7. Phone number validation.
        //8. Validation if the email domain is reachable - SignalR
        //9. Logging Erros in .log files or app console for easier debugging and monitoring
        //10. Implement memory caching for username, email and tokens(to reduce db calls).

        public async Task<ApiResponse<AuthResponseDto>> Register(UserRegisterRequestDTO requestUser)
        {
            //VALIDATIONS
            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.InvalidEmail, ResponseCodes.InvalidEmail);
            }

            if (!_passwordValidation.IsStrong(requestUser.Password))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.WeakPassword, ResponseCodes.WeakPassword);
            }
            
            //CHECK IF USERNAME OR EMAIL ALREADY EXISTS
            (bool usernameExists, bool emailExists) = await _userRepository.CheckUserExistenceAsync(requestUser.Username, requestUser.Email);
             
            switch(true)
            {
                case true when usernameExists:
                    return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UsernameExists, ResponseCodes.UsernameExists);
                case true when emailExists:
                    return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.EmailExists, ResponseCodes.EmailExists);
                case true when usernameExists && emailExists:
                    return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UsernameAndEmailExists, ResponseCodes.UsernameAndEmailExists);
            }

            requestUser.Password = BCrypt.Net.BCrypt.HashPassword(requestUser.Password);         
            User user = _authFactory.Map(requestUser);

            AuthResponseDto tokens = GenerateAndAttachTokens(user);

            bool isRegistrationSuccess = await _userRepository.AddUserAsync(user);

            if (!isRegistrationSuccess)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.RegistratoinFailed, ResponseCodes.RegistrationFailed);
            }

            return ApiResponse<AuthResponseDto>.Success(tokens ,ResponseMessages.UserRegistered);
        }

        public async Task<ApiResponse<AuthResponseDto>> Login(UserLoginRequestDTO requestUser)
        {
            User? user = await _userRepository.GetUserByIdentifier(requestUser.Username);

            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UserNotFound, ResponseCodes.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(requestUser.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.InvalidCredentials, ResponseCodes.InvalidCredentials);
            }

            AuthResponseDto tokens = GenerateAndAttachTokens(user);

            return ApiResponse<AuthResponseDto>.Success(tokens, ResponseMessages.UserLogged);
        }

        private AuthResponseDto GenerateAndAttachTokens(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var (rawRefreshToken, refreshToken) = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            user.RefreshTokens.Add(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenExpirationMinutes)
            };
        }
    }
}
