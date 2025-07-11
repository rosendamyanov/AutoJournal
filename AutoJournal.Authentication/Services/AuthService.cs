using AutoJournal.Authentication.Configuration;
using AutoJournal.Authentication.Factory.Interfaces;
using AutoJournal.Authentication.Services.Interfaces;
using AutoJournal.Authentication.Validation.Interfaces;
using AutoJournal.Common.Response;
using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Interfaces;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoJournal.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
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
            _authRepository = userRepository;
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
            if (!_emailValidation.IsEmailValid(requestUser.Email))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.InvalidEmail, ResponseCodes.InvalidEmail);
            }

            if (!_passwordValidation.IsStrong(requestUser.Password))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.WeakPassword, ResponseCodes.WeakPassword);
            }
            
            (bool usernameExists, bool emailExists) = await _authRepository.CheckUserExistenceAsync(requestUser.Username, requestUser.Email);
             
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

            AuthResponseDto tokens = await GenerateAndAttachTokens(user);

            bool isRegistrationSuccess = await _authRepository.AddUserAsync(user);

            if (!isRegistrationSuccess)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.RegistratoinFailed, ResponseCodes.RegistrationFailed);
            }

            return ApiResponse<AuthResponseDto>.Success(tokens ,ResponseMessages.UserRegistered);
        }

        public async Task<ApiResponse<AuthResponseDto>> Login(UserLoginRequestDTO requestUser)
        {
            User? user = await _authRepository.GetUserByIdentifier(requestUser.Username);

            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UserNotFound, ResponseCodes.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(requestUser.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.InvalidCredentials, ResponseCodes.InvalidCredentials);
            }

            AuthResponseDto tokens = await GenerateAndAttachTokens(user);

            await _authRepository.SaveUserRefreshTokenAsync(user);

            return ApiResponse<AuthResponseDto>.Success(tokens, ResponseMessages.UserLogged);
        }

        public async Task<ApiResponse<AuthResponseDto>> RefreshToken(RefreshRequestDto refresh)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = _tokenService.GetPrincipalFromToken(refresh.AccessToken);
            }
            catch (SecurityTokenException)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.InvalidAccessToken, ResponseCodes.InvalidAccessToken);
            }

            var username = principal.Identity?.Name;

            if (username == null)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UsernameNotFound, ResponseCodes.UsernameNotFound);
            }

            User? user = await _authRepository.GetUserRefreshToken(username);

            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.UserNotFound, ResponseCodes.UserNotFound);
            }

            RefreshToken? storedRefreshToken = await _authRepository.GetRefreshTokenByIdAsync(refresh.RefreshTokenId, user.Id);

            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.RefreshIsRevokedOrExpired, ResponseCodes.RefreshIsRevokedOrExpired);
            }

            if (!BCrypt.Net.BCrypt.Verify(refresh.RefreshToken, storedRefreshToken.TokenHash))
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.RefreshNotFound, ResponseCodes.RefreshNotFound);
            }

            storedRefreshToken.IsRevoked = true;
            
            RevokedToken revokedToken = _authFactory.Map(storedRefreshToken);

            bool result = await _authRepository.SaveRevokedRefreshTokenAsync(revokedToken, storedRefreshToken);

            if (!result)
            {
                return ApiResponse<AuthResponseDto>.Failure(ResponseMessages.RefreshRevokeFailed, ResponseCodes.RefreshRevokeFailed); 
            }

            AuthResponseDto tokens = await GenerateAndAttachTokens(user);

            return ApiResponse<AuthResponseDto>.Success(tokens, ResponseMessages.TokenRefreshed);
        }

        private async Task<AuthResponseDto> GenerateAndAttachTokens(User user)
        {
            string accessToken = _tokenService.GenerateAccessToken(user);
            (string rawRefreshToken, RefreshToken refreshToken) = _tokenService.GenerateRefreshToken();


            refreshToken.UserId = user.Id;
            await _authRepository.SaveRefreshTokenAsync(refreshToken);

            AuthResponseDto tokens = _authFactory.Map(
                accessToken, 
                rawRefreshToken, 
                refreshToken.Id, 
                DateTime.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenExpirationMinutes));

            return tokens;
        }
    }
}
