using Domain.Enums;

namespace Application.DTOs;

public class FacturaDto
{
    public int FacturaId { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public int OrdenServicioId { get; set; }
    public int ClienteId { get; set; }
    public string? NombreCliente { get; set; }
    public DateTime FechaEmision { get; set; }
    public decimal SubtotalRepuestos { get; set; }
    public decimal SubtotalManoObra { get; set; }
    public decimal IVA { get; set; }
    public decimal Descuento { get; set; }
    public decimal MontoTotal { get; set; }
    public FormaPago? FormaPago { get; set; }
    public EstadoFactura Estado { get; set; }
    public string? Observaciones { get; set; }
}

public class CreateFacturaDto
{
    public int OrdenServicioId { get; set; }
    public int ClienteId { get; set; }
    public decimal SubtotalRepuestos { get; set; }
    public decimal SubtotalManoObra { get; set; }
    public decimal IVA { get; set; }
    public decimal Descuento { get; set; }
    public FormaPago? FormaPago { get; set; }
    public string? Observaciones { get; set; }
}

public class UpdateFacturaDto
{
    public EstadoFactura Estado { get; set; }
    public FormaPago? FormaPago { get; set; }
    public string? Observaciones { get; set; }
}