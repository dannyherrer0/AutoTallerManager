using System;

namespace Domain.Entities;

public class GenerarFactura
{
    public decimal CostoManoObra { get; set; }
    public Factura? Factura { get; set; }
    public int SubtotalRepuestos { get; set; }
    public Repuesto? Repuesto { get; set; }
    public OrdenServicio OrdenServicio { get; set; } = null!;
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();

}
