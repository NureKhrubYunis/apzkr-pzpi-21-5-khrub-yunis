using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetCurrentUserAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            return new User
            {
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Type = user.Type,
                Role = user.Role,
                RegistrationDate = user.RegistrationDate
            };
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new User
            {
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Type = user.Type,
                Role = user.Role,
                RegistrationDate = user.RegistrationDate
            }).ToList();
        }
    }

}
