using System;
using System.Threading.Tasks;
using Medoxity.Models;
using Medoxity.Repositories;
using Medoxity.Services;
using Moq;
using Xunit;

namespace Medoxity.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetCurrentUserAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var user = new User
            {
                Username = username,
                Name = "Test User",
                Email = "test@example.com",
                Type = "Regular",
                Role = "User",
                RegistrationDate = System.DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetCurrentUserAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetCurrentUserAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var username = "nonexistentuser";

            _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetCurrentUserAsync(username);

            // Assert
            Assert.Null(result);
        }
    }
}
