using System;

namespace Domain.Entities;

public class CerrarOrdenServicio
{
    public decimal CostoManoObra { get; set; }
    public OrdenServicio OrdenServicio { get; set; } = null!;
    public int SubtotalRepuestos { get; set; }
    public Repuesto? Repuesto { get; set; }
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
}
