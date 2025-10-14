using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Domain.Entities;

public class Auditoria
{
    public int AuditoriaId { get; set; }
    public int? UsuarioId { get; set; }
    public string Entidad { get; set; } = string.Empty;
    public int EntidadId { get; set; }
    public string TipoAccion { get; set; } = string.Empty;
    public string? ValoresAnteriores { get; set; }
    public string? ValoresNuevos { get; set; }
    public string? DireccionIP { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public Usuario? Usuario { get; set; }
}
