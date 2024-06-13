using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync(string username);
        Task<List<User>> GetAllUsersAsync();
    }
}
