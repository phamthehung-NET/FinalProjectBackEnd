using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FinalProjectBackEnd.Services
{
    public class SignalR : Hub
    {
        private readonly ICommentRepository commentRepository;
        public SignalR(ICommentRepository _commentRepository)
        {
            commentRepository = _commentRepository;
        }
        public async Task SendMsg(MessageDTO message)
        {
            await Clients.All.SendAsync("ReceiveMsg", message);
        }

        public async Task SendComment(CommentDTO comment)
        {
            await Clients.All.SendAsync("ReceiveComment", comment);
        }

        public async Task SendReplyComment(ReplyCommentDTO replyComment)
        {
            await Clients.All.SendAsync("ReceiveReplyComment", replyComment);
        }
    }
}
