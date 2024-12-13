using SmartAgroAPI.Contexts;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Models;
using SmartAgroAPI.Services;

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

        public List<NotificationFetchDTO> GetNotificationsFromAnUserInADate(Guid userId, DateTime? lastUpdate)
        {
            var notifications = _context.Notificacaos.Where(x => (x.UsuarioId == userId) && (lastUpdate == null || x.DataCriacao >= lastUpdate)).ToList();
            var notificationsReturn = new List<NotificationFetchDTO>();
            foreach (var item in notifications)
            {
                var itemlog = _context.LogsSensors.Find(item.LogsSensorId);
                itemlog.Sensor = null;
                var itemSensor = _context.Sensors.Find(itemlog.SensorId);
                itemlog.Notificacaos = null;
                itemSensor.LogsSensors = null;

                notificationsReturn.Add(new NotificationFetchDTO()
                {
                    DataCriacao = item.DataCriacao,
                    Id = item.Id,
                    LogsSensor = itemlog,
                    LogsSensorId = item.LogsSensorId,
                    Mensagem = item.Mensagem,
                    NomeBaia = itemSensor.Nome,
                    SensorId = itemSensor.Id,
                    TipoNotificacaoId = item.TipoNotificacaoId,
                    Propriedade = item.Propriedade,
                    UsuarioId = item.UsuarioId,

                });
            }

            return notificationsReturn;
        }

        public bool IsAnyPropertyDangerousToday(Guid IdUsuario)
        {
            var today = DateTime.Now.Date;
            var todayLogs = _context.LogsSensors.Where(x => x.Sensor.UsuarioId == IdUsuario && x.DataAtualizacao.Value.Date == today).ToList();
            foreach (var log in todayLogs)
            {
                if (DataGenerationService.IsOutOfRange(log.Luminosidade, 1000, 100000))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.TemperaturaAr, 5, 25))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.TemperaturaSolo, 5, 25))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.UmidadeSolo, 40, 85))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.UmidadeAr, 30, 80))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.PhSolo, 5, 12))
                {
                    return true;
                }
                if (DataGenerationService.IsOutOfRange(log.QualidadeAr, 100, 200))
                {
                    return true;
                }
            }
            return false;

        }
    }
}
