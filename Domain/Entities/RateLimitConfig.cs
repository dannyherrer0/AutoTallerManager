using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class RateLimitConfig
{
    public int ConfigId { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public int LimitePorMinuto { get; set; } = 60;
    public int LimitePorHora { get; set; } = 1000;
    public int LimitePorDia { get; set; } = 10000;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
}
