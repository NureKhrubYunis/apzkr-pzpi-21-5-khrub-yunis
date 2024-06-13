using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllDocumentsAsync();
    }
}
