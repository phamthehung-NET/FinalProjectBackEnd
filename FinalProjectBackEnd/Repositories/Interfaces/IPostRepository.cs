using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public Pagination<PostDTO> GetAllPosts(string keyword, int? pageIndex, int? pageSize);

        public IQueryable<PostDTO> GetPostDetail(int? id);

        public bool AddPost(PostDTO postReq);

        public bool EditPost(PostDTO postReq);

        public bool DeletePost(int id);

        public bool UserLikeAndDisLike(UserLikePostDTO userLikePostReq);

        public IQueryable<dynamic> GetAllUserLikePost(int postId);

        public IQueryable<dynamic> GetAllComments(int postId);

        public IQueryable<dynamic> GetLikedPostByUser();

        public IQueryable<dynamic> GetLikedCommentByUser();

        public Pagination<PostDTO> GetPostOfUser(string keyword, int? pageIndex, int? pageSize, string userId);
    }
}
