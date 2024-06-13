using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IDrugService
    {
        Task<Drug> GetDrugByIdAsync(int drugID);
        Task<List<Drug>> GetAllDrugsAsync();
    }
}
