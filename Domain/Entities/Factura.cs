using System;
using Domain.Enums;


namespace Domain.Entities;

public class Factura : BaseEntity
{
    public int FacturaId { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public int OrdenServicioId { get; set; }
    public int ClienteId { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
    public decimal SubtotalRepuestos { get; set; }
    public decimal SubtotalManoObra { get; set; }
    public decimal IVA { get; set; }
    public decimal Descuento { get; set; }
    public decimal MontoTotal { get; set; }
    public FormaPago? FormaPago { get; set; }
    public EstadoFactura Estado { get; set; } = EstadoFactura.Pendiente;
    public string? Observaciones { get; set; }

    public OrdenServicio OrdenServicio { get; set; } = null!;
    public Cliente Cliente { get; set; } = null!;

}