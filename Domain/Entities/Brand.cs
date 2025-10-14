using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Brand : BaseEntity
{
    public int BrandId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public string? LogoUrl { get; set; }

    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}