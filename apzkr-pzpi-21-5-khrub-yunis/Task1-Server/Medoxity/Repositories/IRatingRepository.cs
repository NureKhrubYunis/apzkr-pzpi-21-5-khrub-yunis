using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IRatingRepository
    {
        Task DeleteRatingAsync(Rating rating);
        Task<Rating> GetRatingByIdAsync(int ratingID);
        Task UpdateRatingAsync(Rating rating);
        Task<List<Rating>> GetAllRatingsAsync();
    }
}
