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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = Guid.NewGuid(),
                Username = "Test",
                PasswordHash = "123",
                PhoneNumber = "1234567890",
                Email = "test@test.com"
            });

        }


    }
}
