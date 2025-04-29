namespace AutoJournal.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FullPhoneNumber { get; set; }
        public bool PhoneVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";

        public ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}
