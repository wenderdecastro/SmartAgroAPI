using SmartAgroAPI.DataTransferObjects;

namespace SmartAgroAPI.Interfaces
{
    public interface INotificationRepository
    {
        List<NotificationDTO> GetAllNotifications();
        List<NotificationDTO> GetNotificationsFromAnUser(Guid userId);
        List<NotificationFetchDTO> GetNotificationsFromAnUserInADate(Guid userId, DateTime? date);
        void CreateNotification(NotificationCreationDTO notification);

        bool IsAnyPropertyDangerousToday(Guid IdUsuario);
    }
}
