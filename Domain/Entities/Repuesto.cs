using System;

namespace Domain.Entities;

public class Repuesto : BaseEntity
{
    public int RepuestoId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public int CantidadStock { get; set; }
    public int StockMinimo { get; set; } = 5;
    public decimal PrecioUnitario { get; set; }
    public string? Proveedor { get; set; }
    public ICollection<DetalleOrden> DetallesOrden { get; set; } = new List<DetalleOrden>();
}