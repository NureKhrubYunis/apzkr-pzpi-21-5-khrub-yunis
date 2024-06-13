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
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _ratingService = new RatingService(_mockRatingRepository.Object);
        }

        [Fact]
        public async Task GetRatingByIdAsync_RatingExists_ReturnsRating()
        {
            // Arrange
            var ratingID = 1;
            var rating = new Rating { RatingID = ratingID, Value = 4 };

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(ratingID))
                .ReturnsAsync(rating);

            // Act
            var result = await _ratingService.GetRatingByIdAsync(ratingID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingID, result.RatingID);
            Assert.Equal(4, result.Value);
        }

        [Fact]
        public async Task GetRatingByIdAsync_RatingDoesNotExist_ReturnsNull()
        {
            // Arrange
            var ratingID = 1;

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(ratingID))
                .ReturnsAsync((Rating)null);

            // Act
            var result = await _ratingService.GetRatingByIdAsync(ratingID);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateRatingAsync_RatingExists_UpdatesRating()
        {
            // Arrange
            var ratingID = 1;
            var rating = new Rating { RatingID = ratingID, Value = 3 };
            var updatedRating = new Rating { RatingID = ratingID, Value = 5 };

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(ratingID))
                .ReturnsAsync(rating);

            // Act
            await _ratingService.UpdateRatingAsync(updatedRating);

            // Assert
            _mockRatingRepository.Verify(repo => repo.UpdateRatingAsync(It.Is<Rating>(r => r.Value == updatedRating.Value)), Times.Once);
        }

        [Fact]
        public async Task UpdateRatingAsync_RatingDoesNotExist_ThrowsException()
        {
            // Arrange
            var updatedRating = new Rating { RatingID = 1, Value = 5 };

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(updatedRating.RatingID))
                .ReturnsAsync((Rating)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _ratingService.UpdateRatingAsync(updatedRating));
        }

        [Fact]
        public async Task DeleteRatingAsync_RatingExists_DeletesRating()
        {
            // Arrange
            var ratingID = 1;
            var rating = new Rating { RatingID = ratingID };

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(ratingID))
                .ReturnsAsync(rating);

            // Act
            await _ratingService.DeleteRatingAsync(ratingID);

            // Assert
            _mockRatingRepository.Verify(repo => repo.DeleteRatingAsync(rating), Times.Once);
        }

        [Fact]
        public async Task DeleteRatingAsync_RatingDoesNotExist_ThrowsException()
        {
            // Arrange
            var ratingID = 1;

            _mockRatingRepository.Setup(repo => repo.GetRatingByIdAsync(ratingID))
                .ReturnsAsync((Rating)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _ratingService.DeleteRatingAsync(ratingID));
        }
    }
}
