using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;

        public PostService(IPostRepository _postRepository)
        {
            postRepository = _postRepository;
        }

        public bool AddPost(PostDTO postReq)
        {
            var result = postRepository.AddPost(postReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Add fail");
        }

        public bool DeletePost(int id)
        {
            var result = postRepository.DeletePost(id);
            if (result)
            {
                return result;
            }
            throw new Exception("Delete fail");
        }

        public bool EditPost(PostDTO postReq)
        {
            var result = postRepository.EditPost(postReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Edit fail");
        }

        public Pagination<PostDTO> GetAllPosts(string keyword, int? pageIndex, int? pageSize)
        {
            var pagination = postRepository.GetAllPosts(keyword, pageIndex, pageSize);
            if(pagination != null)
            {
                return pagination;
            }
            throw new Exception("Post List is null");
        }

        public IQueryable<PostDTO> GetPostDetail(int id)
        {
            var post = postRepository.GetPostDetail(id);
            if(post == null)
            {
                throw new Exception("Post Not Found");
            }
            return post;
        }

        public bool UserLikeAndDisLike(UserLikePostDTO userLikePostReq)
        {
            var result = postRepository.UserLikeAndDisLike(userLikePostReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot Like Post");
        }

        public IQueryable<dynamic> GetAllUserLikePost(int postId)
        {
            var like = postRepository.GetAllUserLikePost(postId);
            if (like.Any())
            {
                return like;
            }
            throw new Exception("Post has no reaction");
        }

        public IQueryable<dynamic> GetAllComments(int postId)
        {
            var comments = postRepository.GetAllComments(postId);
            if (comments.Any())
            {
                return comments;
            }
            throw new Exception("This post has no Comment");
        }

        public IQueryable<dynamic> GetLikedPostByUser()
        {
            var userLikePost = postRepository.GetLikedPostByUser();
            if (userLikePost.Any())
            {
                return userLikePost;
            }
            throw new Exception("User hasn't like any post");
        }

        public IQueryable<dynamic> GetLikedCommentByUser()
        {
            var userLikeComments = postRepository.GetLikedCommentByUser();
            if (userLikeComments.Any())
            {
                return userLikeComments;
            }
            throw new Exception("User hasn't like any comment");
        }

        public Pagination<PostDTO> GetPostsOfUser(string keyword, int? pageIndex, int? pageSize, string userId)
        {
            var pagination = postRepository.GetPostOfUser(keyword, pageIndex, pageSize, userId);
            if (pagination != null)
            {
                return pagination;
            }
            throw new Exception("Post List is null");
        }
    }
}
