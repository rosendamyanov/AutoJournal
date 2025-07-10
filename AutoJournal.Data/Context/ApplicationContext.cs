using AutoJournal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoJournal.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex(u => u.Username).IsUnique();
                user.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(refreshToken =>
            {
                refreshToken.HasOne(rt => rt.User)
                            .WithMany(u => u.RefreshTokens)
                            .HasForeignKey(rt => rt.UserId);

                refreshToken.HasIndex(rt => rt.TokenHash).IsUnique();
            });

            modelBuilder.Entity<RevokedToken>(revokedToken =>
            {
                revokedToken.HasOne(rt => rt.User)
                            .WithMany(u => u.RevokedTokens)
                            .HasForeignKey(rt => rt.UserId);

                revokedToken.HasIndex(rt => rt.TokenHash);
                revokedToken.HasIndex(rt => rt.RevokedAt);
            });

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = Guid.Parse("a1b2c3d4-1234-5678-9012-abcdef123456"), 
                Username = "test",
                PasswordHash = "123",
                Email = "test@test.com",
                PhoneCountryCode = "359",
                PhoneNumber = "882251418",
                FullPhoneNumber = "359882251418",
                PhoneVerified = false,
                CreatedAt = DateTime.Parse("2025-03-31 13:42:21"),
                Role = "User" 
            });

        }
    }
}
