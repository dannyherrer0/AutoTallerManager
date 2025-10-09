using System;
using Domain.Enums;

namespace Domain.Entities;

public class Usuario : BaseEntity
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
    public ICollection<OrdenServicio> OrdenesComoMecanico { get; set; } = new List<OrdenServicio>();
    public ICollection<OrdenServicio> OrdenesComoRecepcionista { get; set; } = new List<OrdenServicio>();
    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}
