using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public IQueryable<MessageDTO> GetMessageOfConversation(string userId);

        public IQueryable<MessageDTO> GetMessageOfGroupChat(int id);

        public IQueryable<ConversationDTO> GetConversation();

        public IQueryable<GroupChatDTO> GetGroupChat();

        public void CreateGroupChat(GroupChatDTO groupChatReq);

        public IQueryable<GroupChatDTO> GetAllGroupMembers(int id);

        public bool AddMessage(MessageDTO msgReq);

        public bool EditMessage(MessageDTO msgReq);

        public bool RemoveMessage(int id);
    }
}
