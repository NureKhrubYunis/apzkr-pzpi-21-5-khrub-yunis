using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IRatingService
    {
        Task<Rating> GetRatingByIdAsync(int ratingID);
        Task UpdateRatingAsync(Rating updatedRating);
        Task DeleteRatingAsync(int ratingID);
        Task<List<Rating>> GetAllRatingsAsync();
    }
}
