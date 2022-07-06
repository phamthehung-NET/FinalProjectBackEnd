using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        public bool CommentPost(CommentDTO commentReq);

        public bool EditComment(CommentDTO commentReq);

        public bool DeleteComment(int id);

        public IQueryable<CommentDTO> GetCommentDetail(int? id);

        public bool ReplyComment(ReplyCommentDTO replyCommentReq);

        public bool EditReplyComment(ReplyCommentDTO replyCommentReq);

        public bool DeleteReplyComment(int id);

        public IQueryable<dynamic> GetAllCommentLike(int commentId);
    }
}
