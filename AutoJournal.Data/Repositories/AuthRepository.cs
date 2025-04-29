using AutoJournal.Data.Context;
using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AutoJournal.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;

        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(User user)
        {
            _context.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(bool usernameExists, bool emailExists)> CheckUserExistenceAsync(string username, string email)
        {

             var matches = await _context.Users
                .Where(u => u.Username == username || u.Email == email)
                .Select( u => new 
                {
                    u.Username, 
                    u.Email
                })
                .ToListAsync();

            return (
                usernameExists: matches.Any(u => u.Username == username),
                emailExists: matches.Any(u => u.Email == email)
                );
        }

        public async Task<User?> GetUserByIdentifier(string identifier)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken newToken)
        {
            
            var oldTokens = await _context.RefreshTokens
                .Where(t => t.UserId == newToken.UserId && !t.IsRevoked)
                .ToListAsync();

            foreach (var token in oldTokens)
            {
                token.IsRevoked = true;
                
            }

            _context.RefreshTokens.Add(newToken);
            await _context.SaveChangesAsync();
        }
    }
}
