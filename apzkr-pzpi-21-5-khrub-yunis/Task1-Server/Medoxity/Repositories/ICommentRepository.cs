using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByDrugIDAsync(int drugID);
        Task<List<Comment>> GetAllCommentsAsync();
    }
}
