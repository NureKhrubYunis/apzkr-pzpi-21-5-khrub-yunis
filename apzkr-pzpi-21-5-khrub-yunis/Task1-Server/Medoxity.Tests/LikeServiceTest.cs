using Medoxity.Models;
using Medoxity.Repositories;
using Medoxity.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medoxity.Tests
{
    public class LikeServiceTest
    {
        private readonly Mock<ILikeRepository> _mockLikeRepository;
        private readonly LikeService _likeService;

        public LikeServiceTest()
        {
            _mockLikeRepository = new Mock<ILikeRepository>();
            _likeService = new LikeService(_mockLikeRepository.Object);
        }

        [Fact]
        public async Task GetAllLikesAsync_ReturnsAllLikes()
        {
            // Arrange
            var likes = new List<Like>
        {
            new Like { LikeID = 1, Username = "user1", CommentID = 101 },
            new Like { LikeID = 2, Username = "user2", CommentID = 102 },
        };
            _mockLikeRepository.Setup(repo => repo.GetAllLikesAsync()).ReturnsAsync(likes);

            // Act
            var result = await _likeService.GetAllLikesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("user1", result[0].Username);
            Assert.Equal("user2", result[1].Username);
        }
    }
}
