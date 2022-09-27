using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        public Task<bool> CommentPost(CommentDTO commentReq);

        public bool EditComment(CommentDTO commentReq);

        public bool DeleteComment(int id);

        public Task<CommentDTO> GetCommentDetail(int? id);

        public bool UserLikeAndDisLikeComment(UserLikeCommentDTO userLikeCommentReq);

        public bool ReplyComment(ReplyCommentDTO replyCommentReq);

        public bool EditReplyComment(ReplyCommentDTO replyCommentReq);

        public bool DeleteReplyComment(int id);

        public IQueryable<dynamic> GetAllCommentLike(int commentId);
    }
}
