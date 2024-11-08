namespace SmartAgroAPI.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
