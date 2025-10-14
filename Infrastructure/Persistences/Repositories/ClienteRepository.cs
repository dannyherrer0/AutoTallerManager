using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AutoTallerDbContext _context;

    public ClienteRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> GetByIdAsync(int clienteId)
    {
        return await _context.Clientes.FindAsync(clienteId);
    }

    public async Task<Cliente?> GetByIdWithVehiculosAsync(int clienteId)
    {
        return await _context.Clientes
            .Include(c => c.Vehiculos)
                .ThenInclude(v => v.Brand)
            .Include(c => c.OrdenesServicio)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .Where(c => c.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cliente>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Clientes
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Clientes.CountAsync(c => c.Activo);
    }

    public async Task<Cliente?> GetByCorreoAsync(string correo)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Correo == correo && c.Activo);
    }

    public async Task<IEnumerable<Cliente>> SearchByNombreAsync(string nombre)
    {
        return await _context.Clientes
            .Where(c => c.Nombre.Contains(nombre) && c.Activo)
            .ToListAsync();
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Clientes
            .AnyAsync(c => c.Correo == correo);
    }

    public async Task AddAsync(Cliente cliente)
    {
        cliente.FechaRegistro = DateTime.UtcNow;
        await _context.Clientes.AddAsync(cliente);
    }

    public void Update(Cliente cliente)
    {
        cliente.FechaActualizacion = DateTime.UtcNow;
        _context.Clientes.Update(cliente);
    }

    public void Delete(Cliente cliente)
    {
        cliente.Activo = false;
        cliente.FechaActualizacion = DateTime.UtcNow;
        _context.Clientes.Update(cliente);
    }
}