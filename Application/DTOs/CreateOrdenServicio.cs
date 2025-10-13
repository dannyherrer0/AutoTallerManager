namespace Application.DTOs;

public class CreateOrdenServicioDto
{
    public int VehiculoId { get; set; }
    public int ClienteId { get; set; }
    public int? MecanicoId { get; set; }
    public int? RecepcionistaId { get; set; }
    public string? Descripcion { get; set; }
    public decimal CostoManoObra { get; set; }
    public DateTime? FechaEstimadaEntrega { get; set; }
    public int? KilometrajeIngreso { get; set; }
    public List<CreateDetalleOrdenDto>? Detalles { get; set; }
}
