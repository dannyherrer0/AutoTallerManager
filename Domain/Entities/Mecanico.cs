using System;
using System.Dynamic;
using Domain.Enums;
namespace Domain.Entities;
public class Mecanico : BaseEntity
{
    public int MecanicoId { get; set; }
    public string? Nombre { get; set; }
    public EstadoMecanico estado { get; set; } 
}