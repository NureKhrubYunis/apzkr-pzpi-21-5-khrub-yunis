using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MedicalPlatformContext _context;

        public CommentRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByDrugIDAsync(int drugID)
        {
            TimeZoneInfo TimeZone = TimeZoneInfo.Local;

            var comments = await _context.Comments
                .Where(c => c.DrugID == drugID)
                .ToListAsync();

            foreach (var comment in comments)
            {
                comment.CommentDate = TimeZoneInfo.ConvertTimeFromUtc(comment.CommentDate, TimeZone);
            }

            return comments;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

    }
}
