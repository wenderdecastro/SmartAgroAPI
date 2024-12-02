using SmartAgroAPI.Contexts;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly SmartAgroDbContext _context;

        public NotificationRepository(SmartAgroDbContext context)
        {
            _context = context;
        }
        public void CreateNotification(NotificationCreationDTO notification)
        {
            var notificacao = new Notificacao()
            {
                LogsSensorId = notification.LogsSensorId,
                TipoNotificacaoId = notification.TipoNotificacaoId,
                UsuarioId = notification.UsuarioId,
                Mensagem = notification.Mensagem,
                Propriedade = notification.Propriedade,
            };
            _context.Notificacaos.Add(notificacao);
            _context.SaveChanges();
        }

        public List<NotificationDTO> GetAllNotifications()
        {
            var notifications = _context.Notificacaos.Select(x => new NotificationDTO(x)).ToList();

            return notifications;
        }

        public List<NotificationDTO> GetNotificationsFromAnUser(Guid userId)
        {
            var notifications = _context.Notificacaos.Where(x => (x.UsuarioId == userId)).Select(x => new NotificationDTO(x)).ToList();

            return notifications;
        }

        public List<Notificacao> GetNotificationsFromAnUserInADate(Guid userId, DateTime? lastUpdate)
        {
            var notifications = _context.Notificacaos.Where(x => (x.UsuarioId == userId) && (lastUpdate == null || x.DataCriacao >= lastUpdate)).ToList();

            return notifications;
        }
    }
}
