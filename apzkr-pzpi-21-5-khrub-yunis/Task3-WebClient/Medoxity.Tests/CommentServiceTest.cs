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
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _commentService = new CommentService(_mockCommentRepository.Object);
        }

        [Fact]
        public async Task AddCommentAsync_ValidData_CommentAdded()
        {
            // Arrange
            var username = "user";
            var drugID = 1;
            var text = "Great drug!";

            // Act
            await _commentService.AddCommentAsync(username, drugID, text);

            // Assert
            _mockCommentRepository.Verify(repo => repo.AddCommentAsync(It.Is<Comment>(
                c => c.Username == username &&
                     c.DrugID == drugID &&
                     c.Text == text)), Times.Once);
        }

        [Fact]
        public async Task GetCommentsByDrugIDAsync_CommentsExist_ReturnsComments()
        {
            // Arrange
            var drugID = 1;
            var comments = new List<Comment>
        {
            new Comment { DrugID = drugID, Text = "Great drug!" },
            new Comment { DrugID = drugID, Text = "Very effective." }
        };

            _mockCommentRepository.Setup(repo => repo.GetCommentsByDrugIDAsync(drugID))
                .ReturnsAsync(comments);

            // Act
            var result = await _commentService.GetCommentsByDrugIDAsync(drugID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Text == "Great drug!");
            Assert.Contains(result, c => c.Text == "Very effective.");
        }

        [Fact]
        public async Task GetCommentsByDrugIDAsync_NoCommentsExist_ReturnsEmptyList()
        {
            // Arrange
            var drugID = 1;

            _mockCommentRepository.Setup(repo => repo.GetCommentsByDrugIDAsync(drugID))
                .ReturnsAsync(new List<Comment>());

            // Act
            var result = await _commentService.GetCommentsByDrugIDAsync(drugID);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
