using System;

namespace Domain.Entities;

public class Vehiculo : BaseEntity
{
    public int VehiculoId { get; set; }
    public int ClienteId { get; set; }
    public int BrandId { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string VIN { get; set; } = string.Empty;//este es el id del vehiculo  o bueno, es como un serial de vehiculo
    public string? NumeroPlaca { get; set; }
    public int Kilometraje { get; set; }
    public string? Color { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

   
    public Cliente Cliente { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
}