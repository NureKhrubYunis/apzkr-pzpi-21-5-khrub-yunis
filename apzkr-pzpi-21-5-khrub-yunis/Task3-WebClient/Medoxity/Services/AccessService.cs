using Medoxity.Models;
using Medoxity.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Medoxity.Services
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _accessRepository;

        public AccessService(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }

        public async Task<bool> RegisterAsync(Register model)
        {
            var userExists = await _accessRepository.GetUserByUsernameAsync(model.Username);

            if (userExists != null)
            {
                return false;
            }

            var hashedPassword = HashPassword(model.Password);

            await _accessRepository.CreateUserAsync(model.Username, model.Name, hashedPassword, model.Email, "User", "Customer");

            return true;
        }

        public async Task<User> LoginAsync(Login model)
        {
            var hashedPassword = HashPassword(model.Password);

            return await _accessRepository.GetUserByUsernameAndPasswordAsync(model.Username, hashedPassword);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
