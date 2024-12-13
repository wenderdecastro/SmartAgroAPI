using SmartAgroAPI.Models;

namespace SmartAgroAPI.DataTransferObjects
{
    public class NotificationFetchDTO
    {

        public int Id { get; set; }

        public string? Mensagem { get; set; }

        public string? Propriedade { get; set; }

        public Guid? LogsSensorId { get; set; }

        public int SensorId { get; set; }

        public Guid UsuarioId { get; set; }

        public int TipoNotificacaoId { get; set; }

        public DateTime? DataCriacao { get; set; }

        public string NomeBaia { get; set; }

        public virtual LogsSensor? LogsSensor { get; set; }

    }

}
