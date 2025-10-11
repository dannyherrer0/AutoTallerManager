using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FacturaRepository : IFacturaRepository
{
    private readonly AutoTallerDbContext _context;

    public FacturaRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<Factura?> GetByIdAsync(int facturaId)
    {
        return await _context.Facturas.FindAsync(facturaId);
    }

    public async Task<Factura?> GetByIdWithOrdenAsync(int facturaId)
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
                .ThenInclude(o => o.Vehiculo)
                    .ThenInclude(v => v.Cliente)
            .Include(f => f.OrdenServicio)
                .ThenInclude(o => o.DetallesOrden)
                    .ThenInclude(d => d.Repuesto)
            .Include(f => f.Cliente)
            .FirstOrDefaultAsync(f => f.FacturaId == facturaId);
    }

    public async Task<IEnumerable<Factura>> GetAllAsync()
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
            .Include(f => f.Cliente)
            .Where(f => f.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Factura>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.Cliente)
            .Where(f => f.Activo)
            .OrderByDescending(f => f.FechaEmision)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Facturas.CountAsync(f => f.Activo);
    }

    public async Task<Factura?> GetByNumeroFacturaAsync(string numeroFactura)
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
            .Include(f => f.Cliente)
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);
    }

    public async Task<Factura?> GetByOrdenServicioIdAsync(int ordenServicioId)
    {
        return await _context.Facturas
            .FirstOrDefaultAsync(f => f.OrdenServicioId == ordenServicioId);
    }

    public async Task<IEnumerable<Factura>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
            .Where(f => f.ClienteId == clienteId && f.Activo)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<IEnumerable<Factura>> GetByEstadoAsync(EstadoFactura estado)
    {
        return await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.OrdenServicio)
            .Where(f => f.Estado == estado && f.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Factura>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.Facturas
            .Include(f => f.OrdenServicio)
            .Include(f => f.Cliente)
            .Where(f => f.FechaEmision >= fechaInicio && f.FechaEmision <= fechaFin && f.Activo)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalFacturadoEnRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.Facturas
            .Where(f => f.FechaEmision >= fechaInicio && 
                       f.FechaEmision <= fechaFin && 
                       f.Activo &&
                       f.Estado == EstadoFactura.Pagada)
            .SumAsync(f => f.MontoTotal);
    }

    public async Task<string> GenerarNumeroFacturaAsync()
    {
        var fecha = DateTime.Now;
        var prefijo = $"FAC-{fecha:yyyyMM}";
        
        var ultimaFactura = await _context.Facturas
            .Where(f => f.NumeroFactura.StartsWith(prefijo))
            .OrderByDescending(f => f.NumeroFactura)
            .FirstOrDefaultAsync();

        if (ultimaFactura == null)
        {
            return $"{prefijo}-0001";
        }

        var ultimoNumero = int.Parse(ultimaFactura.NumeroFactura.Split('-').Last());
        return $"{prefijo}-{(ultimoNumero + 1):D4}";
    }

    public async Task AddAsync(Factura factura)
    {
        await _context.Facturas.AddAsync(factura);
    }

    public void Update(Factura factura)
    {
        factura.FechaActualizacion = DateTime.UtcNow;
        _context.Facturas.Update(factura);
    }

    public void Delete(Factura factura)
    {
        factura.Activo = false;
        factura.FechaActualizacion = DateTime.UtcNow;
        _context.Facturas.Update(factura);
    }
}
