using System;

namespace Domain.Entities;

public class DetalleOrden
{
    public int DetalleOrdenId { get; set; }
    public int OrdenServicioId { get; set; }
    public int RepuestoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; } = 0;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public OrdenServicio OrdenServicio { get; set; } = null!;
    public Repuesto Repuesto { get; set; } = null!;
}
