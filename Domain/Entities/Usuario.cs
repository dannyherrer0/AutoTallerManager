using System;
using Domain.Enums;

namespace Domain.Entities;

public class Usuario : BaseEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Recepcionista";
    public new bool Activo { get; set; } = true;
    public new DateTime FechaCreacion { get; set; }
    
    // Navegación
    public ICollection<OrdenServicio> OrdenesAsignadas { get; set; } = new List<OrdenServicio>();
}