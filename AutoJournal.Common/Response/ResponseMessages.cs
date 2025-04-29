

namespace AutoJournal.Common.Response
{
    public static class ResponseMessages
    {
        //Success Messages
        public const string UserRegistered = "User registered successfully.";
        public const string UserLogged = "Successfully logged in.";


        //Failure Messages
        public const string UsernameExists = "Username already exists.";
        public const string EmailExists = "Email already exists.";
        public const string UsernameAndEmailExists = "Username and email exists.";
        public const string UserNotFound = "User not found";
        public const string InvalidEmail = "Email is invalid.";
        public const string RegistratoinFailed = "Registration failed, please try again later.";
        public const string InvalidCredentials = "Wrong username or password";
        public const string WeakPassword = "Password must contain uppercase, lowercase, number, and special character";
    }
}
