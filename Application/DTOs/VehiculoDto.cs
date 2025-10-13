namespace Application.DTOs;

public class VehiculoDto
{
    public int VehiculoId { get; set; }
    public int ClienteId { get; set; }
    public string? NombreCliente { get; set; }
    public int BrandId { get; set; }
    public string? NombreMarca { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string? NumeroPlaca { get; set; }
    public int Kilometraje { get; set; }
    public string? Color { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class CreateVehiculoDto
{
    public int ClienteId { get; set; }
    public int BrandId { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string? NumeroPlaca { get; set; }
    public int Kilometraje { get; set; }
    public string? Color { get; set; }
}

public class UpdateVehiculoDto
{
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string? NumeroPlaca { get; set; }
    public int Kilometraje { get; set; }
    public string? Color { get; set; }
}