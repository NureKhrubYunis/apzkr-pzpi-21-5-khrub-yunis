
using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IAccessRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByUsernameAndPasswordAsync(string username, string password); 
        Task CreateUserAsync(string username, string name, string password, string email, string type, string role);
    }
}
