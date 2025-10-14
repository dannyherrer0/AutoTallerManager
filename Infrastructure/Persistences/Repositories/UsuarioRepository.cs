using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AutoTallerDbContext _context;

    public UsuarioRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .Where(u => u.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Usuarios
            .Where(u => u.Activo)
            .OrderBy(u => u.Nombre)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Usuarios.CountAsync(u => u.Activo);
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);
    }

    public async Task<IEnumerable<Usuario>> GetByRolAsync(string rol)
    {
        return await _context.Usuarios
            .Where(u => u.Rol == rol && u.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetMecanicosDisponiblesAsync()
    {
        var mecanicos = await _context.Usuarios
            .Where(u => u.Rol == "Mecánico" && u.Activo)
            .ToListAsync();

        var mecanicosDisponibles = new List<Usuario>();
        
        foreach (var mecanico in mecanicos)
        {
            var ordenesActivas = await _context.OrdenesServicio
                .CountAsync(o => o.MecanicoId == mecanico.UsuarioId && 
                               o.Activo &&
                               (o.Estado == Domain.Enums.EstadoOrden.Pendiente || 
                                o.Estado == Domain.Enums.EstadoOrden.EnProceso));
            
            if (ordenesActivas < 5)
            {
                mecanicosDisponibles.Add(mecanico);
            }
        }

        return mecanicosDisponibles;
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Correo == correo);
    }

    public async Task<bool> ValidarCredencialesAsync(string correo, string passwordHash)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Correo == correo && u.PasswordHash == passwordHash && u.Activo);
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public void Update(Usuario usuario)
    {
        usuario.FechaActualizacion = DateTime.UtcNow;
        _context.Usuarios.Update(usuario);
    }

    public void Delete(Usuario usuario)
    {
        usuario.Activo = false;
        usuario.FechaActualizacion = DateTime.UtcNow;
        _context.Usuarios.Update(usuario);
    }
}