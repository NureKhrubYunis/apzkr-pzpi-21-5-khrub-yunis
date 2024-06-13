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
    public class DrugServiceTests
    {
        private readonly Mock<IDrugRepository> _mockDrugRepository;
        private readonly DrugService _drugService;

        public DrugServiceTests()
        {
            _mockDrugRepository = new Mock<IDrugRepository>();
            _drugService = new DrugService(_mockDrugRepository.Object);
        }

        [Fact]
        public async Task GetDrugByIdAsync_DrugExists_ReturnsDrug()
        {
            // Arrange
            var drugID = 1;
            var drug = new Drug
            {
                DrugID = drugID,
                DrugName = "Test Drug",
                Description = "Test Description",
                Manufacturer = "Test Manufacturer"
            };

            _mockDrugRepository.Setup(repo => repo.GetDrugByIdAsync(drugID))
                .ReturnsAsync(drug);

            // Act
            var result = await _drugService.GetDrugByIdAsync(drugID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(drugID, result.DrugID);
            Assert.Equal("Test Drug", result.DrugName);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal("Test Manufacturer", result.Manufacturer);
        }

        [Fact]
        public async Task GetDrugByIdAsync_DrugDoesNotExist_ReturnsNull()
        {
            // Arrange
            var drugID = 1;

            _mockDrugRepository.Setup(repo => repo.GetDrugByIdAsync(drugID))
                .ReturnsAsync((Drug)null);

            // Act
            var result = await _drugService.GetDrugByIdAsync(drugID);

            // Assert
            Assert.Null(result);
        }
    }
}
