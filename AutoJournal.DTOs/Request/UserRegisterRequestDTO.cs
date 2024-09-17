namespace AutoJournal.DTOs.Request
{
    public class UserRegisterRequestDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
