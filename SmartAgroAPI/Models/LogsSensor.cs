using System;
using System.Collections.Generic;

namespace SmartAgroAPI.Models;

public partial class LogsSensor
{
    public int Id { get; set; }

    public int? SensorId { get; set; }

    public decimal? UmidadeSolo { get; set; }

    public decimal? TemperaturaAr { get; set; }

    public decimal? UmidadeAr { get; set; }

    public decimal? Luminosidade { get; set; }

    public decimal? TemperaturaSolo { get; set; }

    public decimal? QualidadeAr { get; set; }

    public decimal? PhSolo { get; set; }

    public DateTime? DataAtualizacao { get; set; }

    public virtual Sensor? Sensor { get; set; }
}
