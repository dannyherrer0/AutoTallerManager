namespace Application.DTOs;

public class ClienteDto
{
    public int ClienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class CreateClienteDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
}

public class UpdateClienteDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
}
