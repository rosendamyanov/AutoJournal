using AutoJournal.Data.Context;
using AutoJournal.Data.Models;
using AutoJournal.Data.Repositories.Interfaces;
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

        public async Task<bool> AddUserAsync(User user)
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
            return await _context.Users
                                 .Include(u => u.RefreshTokens)
                                 .FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier);
        }

        public async Task<User?> GetUserRefreshToken(string username)
        {
            return await _context.Users
                                 .Include(r => r.RefreshTokens)
                                 .Include(r => r.RevokedTokens)
                                 .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId, Guid userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId && rt.UserId == userId);
        }

        public async Task<bool> SaveRevokedRefreshTokenAsync(RevokedToken revokedToken, RefreshToken refreshToken)
        {
            _context.Update(refreshToken);
            _context.Add(revokedToken);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.Add(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveUserRefreshTokenAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
