using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface IMessageService
    {
        public Pagination<dynamic> GetAllConversationAndGroupChat(int? pageIndex, int? itemPerPage);

        public IQueryable<MessageDTO> GetMessageOfConversation(string userId);

        public IQueryable<MessageDTO> GetMessageOfGroupChat(int id);

        public void CreateGroupChat(GroupChatDTO groupChatReq);

        public IQueryable<GroupChatDTO> GetAllGroupMembers(int id);

        public Task<bool> AddMessage(MessageDTO msgReq);

        public bool EditMessage(MessageDTO msgReq);

        public bool RemoveMessage(int id);
    }
}
