using System;
using System.Collections.Generic;

namespace SmartAgroAPI.Models;

public partial class Notificacao
{
    public int Id { get; set; }

    public string? MensagemCustomizada { get; set; }

    public int SensorId { get; set; }

    public Guid UsuarioId { get; set; }

    public int TipoNotificacaoId { get; set; }

    public DateTime? DataCriacao { get; set; }

    public virtual Sensor Sensor { get; set; } = null!;

    public virtual NotificacaoStatus TipoNotificacao { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
