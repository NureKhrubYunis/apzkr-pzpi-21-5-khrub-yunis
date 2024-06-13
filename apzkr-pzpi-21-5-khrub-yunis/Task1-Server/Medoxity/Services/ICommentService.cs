using Medoxity.Models;

namespace Medoxity.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(string username, int drugID, string text);
        Task<IEnumerable<Comment>> GetCommentsByDrugIDAsync(int drugID);
        Task<List<Comment>> GetAllCommentsAsync();
    }
}
