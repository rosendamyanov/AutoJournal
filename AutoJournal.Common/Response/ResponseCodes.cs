

namespace AutoJournal.Common.Response
{
    public static class ResponseCodes
    {
        public const string UsernameExists = "USERNAME_EXISTS";
        public const string EmailExists = "EMAIL_EXISTS";
        public const string UsernameAndEmailExists = "USERNAME_EMAIL_EXISTS";
        public const string InvalidEmail = "EMAIL_INVALID";
        public const string RegistrationFailed = "REGISTRATION_FAILED";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string WeakPassword = "WEAK_PASSWORD";
    }
}
