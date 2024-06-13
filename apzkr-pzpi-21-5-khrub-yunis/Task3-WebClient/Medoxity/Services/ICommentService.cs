using Medoxity.Models;

namespace Medoxity.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(string username, int drugID, string text);
        Task<IEnumerable<Comment>> GetCommentsByDrugIDAsync(int drugID);
        Task DeleteCommentAsync(int commentId);
        Task<List<Comment>> GetAllCommentsAsync();
    }
}
