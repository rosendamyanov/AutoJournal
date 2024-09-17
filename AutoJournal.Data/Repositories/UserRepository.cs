using AutoJournal.Data.Context;
using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AutoJournal.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(User user)
        {
            _context.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExists(string username, string email)
        {
            return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
        }
    }
}
