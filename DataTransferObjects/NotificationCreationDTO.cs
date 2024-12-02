using SmartAgroAPI.Models;

namespace SmartAgroAPI.DataTransferObjects
{
    public class NotificationCreationDTO
    {

        public NotificationCreationDTO(Notificacao notificacao)
        {
            Mensagem = notificacao.Mensagem;
            Propriedade = notificacao.Propriedade;
            LogsSensorId = notificacao.LogsSensorId;
            UsuarioId = notificacao.UsuarioId;
            TipoNotificacaoId = notificacao.TipoNotificacaoId;
        }
        public string? Mensagem { get; set; }

        public string? Propriedade { get; set; }

        public int LogsSensorId { get; set; }

        public Guid UsuarioId { get; set; }

        public int TipoNotificacaoId { get; set; }



    }
}
