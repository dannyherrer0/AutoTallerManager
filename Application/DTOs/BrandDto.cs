namespace Application.DTOs;

public class BrandDto
{
    public int BrandId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public string? LogoUrl { get; set; }
}

public class CreateBrandDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public string? LogoUrl { get; set; }
}

public class UpdateBrandDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public string? LogoUrl { get; set; }
}