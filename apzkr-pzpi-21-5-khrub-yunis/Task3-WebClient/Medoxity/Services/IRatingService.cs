using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IRatingService
    {
        Task AddRatingAsync(string username, int drugId, int value);
        Task<Rating> GetRatingByIdAsync(int ratingID);
        Task UpdateRatingAsync(Rating updatedRating);
        Task DeleteRatingAsync(int ratingID);
        Task<List<Rating>> GetAllRatingsAsync();
        Task<Rating> GetRatingByUserAndDrugAsync(string username, int drugId);
        Task<RatingInfo> CheckUserRatingForDrugAsync(string username, int drugId);
    }
}
