using Medoxity.Models;

namespace Medoxity.Services
{
    public interface ILikeService
    {
        Task<List<Like>> GetAllLikesAsync();
    }
}
