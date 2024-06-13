using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetDocumentsByUsernameAsync(string username);
        Task<List<Document>> GetAllDocumentsAsync();
    }
}
