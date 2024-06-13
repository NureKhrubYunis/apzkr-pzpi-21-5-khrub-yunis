using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
        public class LikeService : ILikeService
        {
            private readonly ILikeRepository _likeRepository;

            public LikeService(ILikeRepository likeRepository)
            {
                _likeRepository = likeRepository;
            }

            public async Task<List<Like>> GetAllLikesAsync()
            {
                var likes = await _likeRepository.GetAllLikesAsync();
                return likes.Select(like => new Like
                {
                    LikeID = like.LikeID,
                    Username = like.Username,
                    CommentID = like.CommentID
                }).ToList();
            }
    }
}
