using System;

namespace Domain.Entities;

public class DetalleOrden
{
    public int DetalleOrdenId { get; set; }
    public int OrdenServicioId { get; set; }
    public int RepuestoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario; 
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public OrdenServicio OrdenServicio { get; set; } = null!;
    public Repuesto Repuesto { get; set; } = null!;
}
