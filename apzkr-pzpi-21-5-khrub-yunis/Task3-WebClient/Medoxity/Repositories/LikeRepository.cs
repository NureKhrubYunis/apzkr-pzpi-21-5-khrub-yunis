using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MedicalPlatformContext _context;

        public LikeRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task<List<Like>> GetAllLikesAsync()
        {
            return await _context.Likes.ToListAsync();
        }
    }
}
