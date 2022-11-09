using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public IQueryable<MessageDTO> GetMessageOfConversation(string userId);

        public IQueryable<MessageDTO> GetMessageOfConversation(int conversationId);

        public IQueryable<MessageDTO> GetMessageOfGroupChat(int id);

        public IQueryable<dynamic> GetConversation();

        public IQueryable<dynamic> GetGroupChat();

        public void CreateGroupChat(GroupChatDTO groupChatReq);

        public IQueryable<GroupChatDTO> GetAllGroupMembers(int id);

        public Task<bool> AddMessage(MessageDTO msgReq);

        public bool EditMessage(MessageDTO msgReq);

        public bool RemoveMessage(int id);

        public bool AddConversation(string userId);

        public IQueryable<UserDTO> GetAllFriendForGroupChat();
    }
}
