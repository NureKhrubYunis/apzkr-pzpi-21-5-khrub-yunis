using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IDrugRepository
    {
        Task<Drug> GetDrugByIdAsync(int drugID);
        Task<List<Drug>> GetAllDrugsAsync();

    }
}
