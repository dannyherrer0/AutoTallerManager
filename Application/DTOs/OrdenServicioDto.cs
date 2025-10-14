using System;
using Domain.Entities;
namespace Application.DTOs;


public class OrdenServicioDto
{
    public int OrdenServicioId { get; set; }
    public int MecanicoId { get; set; }
    public Usuario Mecanico { get; set; } 
    public int VehiculoId { get; set; }
    public string? VehiculoInfo { get; set; } // Nuevo campo para MapFrom
    public string? NombreCliente { get; set; }
    public string? NombreMecanico { get; set; } // Nuevo
    public string? NombreRecepcionista { get; set; } // Nuevo
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public string? Estado { get; set; }
    public decimal Total { get; set; }
    public List<DetalleOrdenDto> Detalles { get; set; } = new();
    public string? ModeloVehiculo { get; set; }

}
