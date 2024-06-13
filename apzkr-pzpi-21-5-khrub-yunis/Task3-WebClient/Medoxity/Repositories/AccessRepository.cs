using Medoxity.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Medoxity.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly MedicalPlatformContext _context;

        public AccessRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FromSqlInterpolated($"SELECT * FROM Users WHERE Username = {username}")
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Users
                .FromSqlInterpolated($"SELECT * FROM Users WHERE Username = {username} AND Password = {password}")
                .FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(string username, string name, string password, string email, string type, string role)
        {
            var UsernameArg = new SqlParameter("@param1", SqlDbType.VarChar) { Value = username };
            var NameArg = new SqlParameter("@param2", SqlDbType.VarChar) { Value = name };
            var PasswordArg = new SqlParameter("@param3", SqlDbType.VarChar) { Value = password };
            var EmailArg = new SqlParameter("@param4", SqlDbType.VarChar) { Value = email };
            var TypeArg = new SqlParameter("@param5", SqlDbType.VarChar) { Value = type };
            var RoleArg = new SqlParameter("@param6", SqlDbType.VarChar) { Value = role };

            await _context.Database.ExecuteSqlRawAsync("EXEC CreateUser @param1, @param2, @param3, @param4, @param5, @param6",
                UsernameArg, NameArg, PasswordArg, EmailArg, TypeArg, RoleArg);
        }
    }
}
