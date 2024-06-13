using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MedicalPlatformContext _context;

        public RatingRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Rating rating)
        {
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.Username == rating.Username && r.DrugID == rating.DrugID);

            if (existingRating == null)
            {
                _context.Ratings.Add(rating);
            }
            else
            {
                existingRating.Value = rating.Value;
                _context.Ratings.Update(existingRating);
            }
            await _context.SaveChangesAsync();
        }


        public async Task<Rating> GetRatingByIdAsync(int ratingID)
        {
            return await _context.Ratings.FindAsync(ratingID);
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRatingAsync(Rating rating)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rating>> GetAllRatingsAsync()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating> GetRatingByUserAndDrugAsync(string username, int drugId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.Username == username && r.DrugID == drugId);
        }
    }
}
