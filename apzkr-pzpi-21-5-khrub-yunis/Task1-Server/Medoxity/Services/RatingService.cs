using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<Rating> GetRatingByIdAsync(int ratingID)
        {
            return await _ratingRepository.GetRatingByIdAsync(ratingID);
        }

        public async Task UpdateRatingAsync(Rating updatedRating)
        {
            var rating = await _ratingRepository.GetRatingByIdAsync(updatedRating.RatingID);
            if (rating == null)
            {
                throw new Exception("Rating not found.");
            }

            rating.Value = updatedRating.Value;
            await _ratingRepository.UpdateRatingAsync(rating);
        }

        public async Task DeleteRatingAsync(int ratingID)
        {
            var rating = await _ratingRepository.GetRatingByIdAsync(ratingID);
            if (rating == null)
            {
                throw new Exception("Rating not found.");
            }

            await _ratingRepository.DeleteRatingAsync(rating);
        }

        public async Task<List<Rating>> GetAllRatingsAsync()
        {
            var ratings = await _ratingRepository.GetAllRatingsAsync();
            return ratings.Select(rating => new Rating
            {
                RatingID = rating.RatingID,
                Username = rating.Username,
                DrugID = rating.DrugID,
                Value = rating.Value
            }).ToList();
        }
    }
}
