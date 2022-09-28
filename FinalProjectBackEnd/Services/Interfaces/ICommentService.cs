using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<bool> CommentPost(CommentDTO commentReq);

        public bool EditComment(CommentDTO commentReq);

        public bool DeleteComment(int id);

        public Task<CommentDTO> GetCommentDetail(int id);

        public bool UserLikeAndDisLikeComment(UserLikeCommentDTO userLikeCommentReq);

        public Task<bool> ReplyComment(ReplyCommentDTO replyCommentReq);

        public bool EditReplyComment(ReplyCommentDTO replyCommentReq);

        public bool DeleteReplyComment(int id);

        public IQueryable<dynamic> GetAllCommentLike(int commentId);

        public IQueryable<ReplyCommentDTO> GetAllReplyComment(int commentId);
    }
}
