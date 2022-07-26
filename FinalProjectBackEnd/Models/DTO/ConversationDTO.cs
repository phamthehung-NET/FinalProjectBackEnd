namespace FinalProjectBackEnd.Models.DTO
{
    public class ConversationDTO
    {
        public int? Id { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }
        
        public dynamic LastestMessage { get; set; }
    }
}
