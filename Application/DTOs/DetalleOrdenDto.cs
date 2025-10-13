namespace Application.DTOs;

public class DetalleOrdenDto
{
    public int DetalleOrdenId { get; set; }
    public int RepuestoId { get; set; }
    public string? CodigoRepuesto { get; set; }
    public string? DescripcionRepuesto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class CreateDetalleOrdenDto
{
    public int RepuestoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}