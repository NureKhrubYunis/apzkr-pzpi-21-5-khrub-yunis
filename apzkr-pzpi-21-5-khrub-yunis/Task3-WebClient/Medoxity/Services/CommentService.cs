using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task AddCommentAsync(string username, int drugID, string text)
        {
            var comment = new Comment
            {
                Username = username,
                DrugID = drugID,
                Text = text,
                CommentDate = DateTime.UtcNow
            };

            await _commentRepository.AddCommentAsync(comment);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByDrugIDAsync(int drugID)
        {
            return await _commentRepository.GetCommentsByDrugIDAsync(drugID);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await _commentRepository.DeleteCommentAsync(commentId);
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return comments.Select(comment => new Comment
            {
                CommentID = comment.CommentID,
                Username = comment.Username,
                DrugID = comment.DrugID,
                Text = comment.Text,
                CommentDate = comment.CommentDate
            }).ToList();
        }
    }
}
