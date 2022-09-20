using FinalProjectBackEnd.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace FinalProjectBackEnd.Services
{
    public class SignalR : Hub
    {
        public async Task SendMsg(MessageDTO message)
        {
            await Clients.All.SendAsync("ReceiveMsg", message);
        }

        public async Task SendComment(CommentDTO comment)
        {
            await Clients.All.SendAsync("ReceiveComment", comment);
        }
    }
}
