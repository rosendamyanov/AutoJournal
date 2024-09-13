using AutoJournal.Data.Context;
using AutoJournal.Data.Models;

namespace AutoJournal.Data.Repositories
{
    public class UserRepository
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
    }
}
