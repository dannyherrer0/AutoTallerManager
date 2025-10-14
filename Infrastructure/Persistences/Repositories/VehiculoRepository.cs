using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly AutoTallerDbContext _context;

    public VehiculoRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<Vehiculo?> GetByIdAsync(int vehiculoId)
    {
        return await _context.Vehiculos.FindAsync(vehiculoId);
    }

    public async Task<Vehiculo?> GetByIdWithClienteAsync(int vehiculoId)
    {
        return await _context.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Brand)
            .FirstOrDefaultAsync(v => v.VehiculoId == vehiculoId);
    }

    public async Task<Vehiculo?> GetByIdWithBrandAsync(int vehiculoId)
    {
        return await _context.Vehiculos
            .Include(v => v.Brand)
            .FirstOrDefaultAsync(v => v.VehiculoId == vehiculoId);
    }

    public async Task<IEnumerable<Vehiculo>> GetAllAsync()
    {
        return await _context.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Brand)
            .Where(v => v.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehiculo>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Brand)
            .Where(v => v.Activo)
            .OrderByDescending(v => v.FechaRegistro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Vehiculos.CountAsync(v => v.Activo);
    }

    public async Task<Vehiculo?> GetByVINAsync(string vin)
    {
        return await _context.Vehiculos
            .FirstOrDefaultAsync(v => v.VIN == vin);
    }

    public async Task<IEnumerable<Vehiculo>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.Vehiculos
            .Include(v => v.Brand)
            .Where(v => v.ClienteId == clienteId && v.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehiculo>> GetByBrandIdAsync(int brandId)
    {
        return await _context.Vehiculos
            .Include(v => v.Cliente)
            .Where(v => v.BrandId == brandId && v.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehiculo>> SearchByMarcaModeloAsync(string searchTerm)
    {
        return await _context.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Brand)
            .Where(v => (v.Modelo.Contains(searchTerm) || v.Brand.Nombre.Contains(searchTerm)) && v.Activo)
            .ToListAsync();
    }

    public async Task<bool> ExisteVINAsync(string vin)
    {
        return await _context.Vehiculos
            .AnyAsync(v => v.VIN == vin);
    }

    public async Task<bool> TieneOrdenesActivasAsync(int vehiculoId)
    {
        return await _context.OrdenesServicio
            .AnyAsync(o => o.VehiculoId == vehiculoId && 
                          o.Activo &&
                          (o.Estado == Domain.Enums.EstadoOrden.Pendiente || 
                           o.Estado == Domain.Enums.EstadoOrden.EnProceso));
    }

    public async Task AddAsync(Vehiculo vehiculo)
    {
        vehiculo.FechaRegistro = DateTime.UtcNow;
        await _context.Vehiculos.AddAsync(vehiculo);
    }

    public void Update(Vehiculo vehiculo)
    {
        vehiculo.FechaActualizacion = DateTime.UtcNow;
        _context.Vehiculos.Update(vehiculo);
    }

    public void Delete(Vehiculo vehiculo)
    {
        vehiculo.Activo = false;
        vehiculo.FechaActualizacion = DateTime.UtcNow;
        _context.Vehiculos.Update(vehiculo);
    }
}