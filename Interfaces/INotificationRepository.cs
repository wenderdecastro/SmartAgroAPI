using SmartAgroAPI.Models;

namespace SmartAgroAPI.Interfaces
{
    public interface INotificationRepository
    {
        List<Notificacao> GetAllNotifications();
        List<Notificacao> GetNotificationsFromUser(Guid userId);
        void CreateNotification();

    }
}
