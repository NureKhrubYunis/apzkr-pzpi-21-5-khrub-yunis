using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class DrugRepository : IDrugRepository
    {
        private readonly MedicalPlatformContext _context;

        public DrugRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task<Drug> GetDrugByIdAsync(int drugID)
        {
            return await _context.Drugs.FindAsync(drugID);
        }

        public async Task<List<Drug>> GetAllDrugsAsync()
        {
            return await _context.Drugs.ToListAsync();
        }

        // Інші методи для роботи з препаратами
    }
}
