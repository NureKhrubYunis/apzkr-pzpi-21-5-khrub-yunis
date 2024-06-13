using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IDocumentService
    {
        Task<List<Document>> GetAllDocumentsAsync();
        Task<IEnumerable<Document>> GetDocumentsByUsernameAsync(string username);
    }
}
