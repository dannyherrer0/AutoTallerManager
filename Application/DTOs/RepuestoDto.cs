namespace Application.DTOs;

public class RepuestoDto
{
    public int RepuestoId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public int CantidadStock { get; set; }
    public int StockMinimo { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string? Proveedor { get; set; }
    public bool BajoStock => CantidadStock <= StockMinimo;
}

public class CreateRepuestoDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public int CantidadStock { get; set; }
    public int StockMinimo { get; set; } = 5;
    public decimal PrecioUnitario { get; set; }
    public string? Proveedor { get; set; }
}

public class UpdateRepuestoDto
{
    public string Descripcion { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int StockMinimo { get; set; }
    public string? Proveedor { get; set; }
}

public class UpdateStockDto
{
    public int Cantidad { get; set; }
}