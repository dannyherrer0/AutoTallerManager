using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrdenServicioRepository : IOrdenServicioRepository
{
    private readonly AutoTallerDbContext _context;

    public OrdenServicioRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<OrdenServicio?> GetByIdAsync(int id)
    {
        return await _context.OrdenesServicio.FindAsync(id);
    }

    public async Task<OrdenServicio?> GetByIdWithDetallesAsync(int id)
    {
        return await _context.OrdenesServicio
            .Include(o => o.DetallesOrden)
                .ThenInclude(d => d.Repuesto)
            .FirstOrDefaultAsync(o => o.OrdenServicioId == id);
    }

    public async Task<OrdenServicio?> GetByIdCompleteAsync(int id)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
                .ThenInclude(v => v.Cliente)
            .Include(o => o.Mecanico)
            .Include(o => o.Recepcionista)
            .Include(o => o.DetallesOrden)
                .ThenInclude(d => d.Repuesto)
            .Include(o => o.Factura)
            .FirstOrDefaultAsync(o => o.OrdenServicioId == id);
    }

    public async Task<IEnumerable<OrdenServicio>> GetAllAsync()
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
                .ThenInclude(v => v.Cliente)
            .Include(o => o.Mecanico)
            .Include(o => o.Recepcionista)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
                .ThenInclude(v => v.Cliente)
            .Include(o => o.Mecanico)
            .OrderByDescending(o => o.FechaIngreso)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.OrdenesServicio.CountAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetByEstadoAsync(string estado)
    {
        // Convertimos el string al enum EstadoOrden
        if (!Enum.TryParse<EstadoOrden>(estado, true, out var estadoEnum))
            return Enumerable.Empty<OrdenServicio>();

        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
                .ThenInclude(v => v.Cliente)
            .Include(o => o.Mecanico)
            .Where(o => o.Estado == estadoEnum)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetByMecanicoIdAsync(int mecanicoId)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
                .ThenInclude(v => v.Cliente)
            .Where(o => o.MecanicoId == mecanicoId)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetByVehiculoIdAsync(int vehiculoId)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Mecanico)
            .Where(o => o.VehiculoId == vehiculoId)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
            .Include(o => o.Mecanico)
            .Where(o => o.ClienteId == clienteId)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenServicio>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.OrdenesServicio
            .Include(o => o.Vehiculo)
            .Include(o => o.Mecanico)
            .Where(o => o.FechaIngreso >= fechaInicio && o.FechaIngreso <= fechaFin)
            .OrderByDescending(o => o.FechaIngreso)
            .ToListAsync();
    }

    public async Task<bool> VehiculoTieneOrdenActivaAsync(int vehiculoId)
    {
        return await _context.OrdenesServicio
            .AnyAsync(o => o.VehiculoId == vehiculoId &&
                          (o.Estado == EstadoOrden.Pendiente || o.Estado == EstadoOrden.EnProceso));
    }

    public async Task<int> GetOrdenesActivasPorMecanicoAsync(int mecanicoId)
    {
        return await _context.OrdenesServicio
            .CountAsync(o => o.MecanicoId == mecanicoId &&
                           (o.Estado == EstadoOrden.Pendiente || o.Estado == EstadoOrden.EnProceso));
    }

    public async Task AddAsync(OrdenServicio orden)
    {
        await _context.OrdenesServicio.AddAsync(orden);
    }

    public void Update(OrdenServicio orden)
    {
        _context.OrdenesServicio.Update(orden);
    }

    public void Delete(OrdenServicio orden)
    {
        _context.OrdenesServicio.Remove(orden);
    }
}
