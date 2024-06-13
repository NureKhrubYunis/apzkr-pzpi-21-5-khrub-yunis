using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(Rating rating);
        Task DeleteRatingAsync(Rating rating);
        Task<Rating> GetRatingByIdAsync(int ratingID);
        Task UpdateRatingAsync(Rating rating);
        Task<List<Rating>> GetAllRatingsAsync();
        Task<Rating> GetRatingByUserAndDrugAsync(string username, int drugId);
    }
}
