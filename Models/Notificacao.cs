namespace SmartAgroAPI.Models;

public partial class Notificacao
{
    public int Id { get; set; }

    public string? Propriedade { get; set; }

    public string? Mensagem { get; set; }

    public int SensorId { get; set; }

    public Guid UsuarioId { get; set; }

    public int StatusId { get; set; }

    public DateTime? DataCriacao { get; set; }

    public virtual Sensor Sensor { get; set; } = null!;

    public virtual NotificacaoStatus Status { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
