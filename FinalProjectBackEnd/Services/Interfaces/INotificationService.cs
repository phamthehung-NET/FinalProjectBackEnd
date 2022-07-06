using FinalProjectBackEnd.Models;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface INotificationService
    {
        public List<Notification> GetAllNotifications();
    }
}
