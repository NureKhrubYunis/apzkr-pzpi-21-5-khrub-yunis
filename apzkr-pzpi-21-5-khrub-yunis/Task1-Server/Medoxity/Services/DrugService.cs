using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _drugRepository;

        public DrugService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }

        public async Task<Drug> GetDrugByIdAsync(int drugID)
        {
            return await _drugRepository.GetDrugByIdAsync(drugID);
        }

        public async Task<List<Drug>> GetAllDrugsAsync()
        {
            var drugs = await _drugRepository.GetAllDrugsAsync();
            return drugs.Select(drug => new Drug
            {
                DrugID = drug.DrugID,
                DrugName = drug.DrugName,
                Description = drug.Description,
                Manufacturer = drug.Manufacturer,
                Price = drug.Price,
                StockQuantity = drug.StockQuantity,
                ReleaseDate = drug.ReleaseDate
            }).ToList();
        }

        
    }
}
