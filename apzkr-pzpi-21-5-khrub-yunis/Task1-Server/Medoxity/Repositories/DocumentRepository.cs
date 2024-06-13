using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MedicalPlatformContext _context;

        public DocumentRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            return await _context.Documents.ToListAsync();
        }
    }
}
