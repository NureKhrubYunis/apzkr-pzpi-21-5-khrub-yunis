using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IAccessService
    {
        Task<bool> RegisterAsync(Register model);
        Task<User> LoginAsync(Login model);
    }
}
