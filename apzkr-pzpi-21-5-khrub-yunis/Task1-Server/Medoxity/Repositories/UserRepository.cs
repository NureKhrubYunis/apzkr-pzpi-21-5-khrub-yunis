using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MedicalPlatformContext _context;

        public UserRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }

}
