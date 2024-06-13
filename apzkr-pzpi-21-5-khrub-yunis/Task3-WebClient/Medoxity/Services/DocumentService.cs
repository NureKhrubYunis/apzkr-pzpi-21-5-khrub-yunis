using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByUsernameAsync(string username)
        {
            return await _documentRepository.GetDocumentsByUsernameAsync(username);
        }

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            var documents = await _documentRepository.GetAllDocumentsAsync();
            return documents.Select(document => new Document
            {
                DocumentID = document.DocumentID,
                Username = document.Username,
                DocumentName = document.DocumentName,
                DocumentType = document.DocumentType,
                DocumentPath = document.DocumentPath,
                UploadDate = document.UploadDate
            }).ToList();
        }
    }
}
