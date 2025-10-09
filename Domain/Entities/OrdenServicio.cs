using System;
using Domain.Enums;


namespace Domain.Entities;

public class OrdenServicio : BaseEntity
{
    public int OrdenServicioId { get; set; }
    public string NumeroOrden { get; set; } = string.Empty;
    public int VehiculoId { get; set; }
    public int? MecanicoId { get; set; }
    public int? RecepcionistaId { get; set; }
    public TipoServicio TipoServicio { get; set; }
    public string? Descripcion { get; set; }
    public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    public DateTime? FechaEstimadaEntrega { get; set; }
    public DateTime? FechaEntregaReal { get; set; }
    public int? KilometrajeIngreso { get; set; }
    public decimal CostoManoObra { get; set; }
    public string? ObservacionesMecanico { get; set; }

    public Vehiculo Vehiculo { get; set; } = null!;
    public Usuario? Mecanico { get; set; }
    public Usuario? Recepcionista { get; set; }
    public ICollection<DetalleOrden> DetallesOrden { get; set; } = new List<DetalleOrden>();
    public Factura? Factura { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
}

