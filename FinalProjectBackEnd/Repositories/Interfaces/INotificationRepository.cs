using FinalProjectBackEnd.Models;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        public List<Notification> GetAllNotifications();

        public bool AddNotification(Notification notification);

        public bool RemoveNotification(Notification notification);

        public void EditNotification(Notification notification, int? status);
    }
}
