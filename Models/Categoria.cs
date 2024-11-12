using System;
using System.Collections.Generic;

namespace SmartAgroAPI.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
