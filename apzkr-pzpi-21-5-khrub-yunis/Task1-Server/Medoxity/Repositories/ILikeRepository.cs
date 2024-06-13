using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface ILikeRepository
    {
        Task<List<Like>> GetAllLikesAsync();
    }
}
