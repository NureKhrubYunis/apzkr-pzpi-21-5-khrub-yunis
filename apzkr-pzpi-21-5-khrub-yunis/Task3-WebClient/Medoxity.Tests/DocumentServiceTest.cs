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
    public class DocumentServiceTest
    {
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly DocumentService _documentService;

        public DocumentServiceTest()
        {
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _documentService = new DocumentService(_mockDocumentRepository.Object);
        }

        [Fact]
        public async Task GetAllDocumentsAsync_ReturnsAllDocuments()
        {
            // Arrange
            var documents = new List<Document>
        {
            new Document { DocumentID = 1, Username = "user1", DocumentName = "Doc1", DocumentType = "Type1", DocumentPath = "/path/to/doc1", UploadDate = DateTime.UtcNow },
            new Document { DocumentID = 2, Username = "user2", DocumentName = "Doc2", DocumentType = "Type2", DocumentPath = "/path/to/doc2", UploadDate = DateTime.UtcNow },
        };
            _mockDocumentRepository.Setup(repo => repo.GetAllDocumentsAsync()).ReturnsAsync(documents);

            // Act
            var result = await _documentService.GetAllDocumentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("user1", result[0].Username);
            Assert.Equal("user2", result[1].Username);
        }
    }
}
