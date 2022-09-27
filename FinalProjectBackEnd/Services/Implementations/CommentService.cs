using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;

        public CommentService(ICommentRepository _commentRepository)
        {
            commentRepository = _commentRepository;
        }

        public async Task<bool> CommentPost(CommentDTO commentReq)
        {
            var result = await commentRepository.CommentPost(commentReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot Comment");
        }

        public bool DeleteComment(int id)
        {
            var result = commentRepository.DeleteComment(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot delete Comment");
        }

        public bool DeleteReplyComment(int id)
        {
            var result = commentRepository.DeleteReplyComment(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot delete Reply Comment");
        }

        public bool EditComment(CommentDTO commentReq)
        {
            var result = commentRepository.EditComment(commentReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot edit Comment");
        }

        public bool EditReplyComment(ReplyCommentDTO replyCommentReq)
        {
            var result = commentRepository.EditReplyComment(replyCommentReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot edit Reply Comment");
        }

        public IQueryable<dynamic> GetAllCommentLike(int commentId)
        {
            var like = commentRepository.GetAllCommentLike(commentId);
            if (like.Any())
            {
                return like;
            }
            throw new Exception("Comment has no reaction");
        }

        public async Task<CommentDTO> GetCommentDetail(int id)
        {
            var comment = await commentRepository.GetCommentDetail(id);
            if (comment != null)
            {
                return comment;
            }
            throw new Exception("Cannot get comment detail");
        }

        public bool ReplyComment(ReplyCommentDTO replyCommentReq)
        {
            var result = commentRepository.ReplyComment(replyCommentReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot Reply Comment");
        }

        public bool UserLikeAndDisLikeComment(UserLikeCommentDTO userLikeCommentReq)
        {
            var result = commentRepository.UserLikeAndDisLikeComment(userLikeCommentReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot Like Post");
        }
    }
}
