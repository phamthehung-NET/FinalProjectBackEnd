using Microsoft.AspNetCore.SignalR;

namespace FinalProjectBackEnd.Services
{
    public class SignalR : Hub
    {
        public async Task SendMsg(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMsg", "haha", message);
        }

        public async Task ReceiveMsg(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMsg", "test", "test message");
        }
    }
}
