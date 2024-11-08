using System;
using System.Collections.Generic;

namespace SmartAgroAPI.Models;

public partial class Usuario
{
    public string? Nome { get; set; }

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string? CodigoVerificacao { get; set; }

    public Guid Id { get; set; }

    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
