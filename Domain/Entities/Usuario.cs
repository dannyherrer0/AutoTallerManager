using System;
using Domain.Enums;

namespace Domain.Entities;

public class Usuario : BaseEntity
{
    public int UsuarioId { get; set; }    
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Recepcionista";
  
    public ICollection<OrdenServicio> OrdenesAsignadas { get; set; } = new List<OrdenServicio>();
}