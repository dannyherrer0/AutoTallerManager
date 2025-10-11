using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepuestoRepository : IRepuestoRepository
{
    private readonly AutoTallerDbContext _context;

    public RepuestoRepository(AutoTallerDbContext context)
    {
        _context = context;
    }

    public async Task<Repuesto?> GetByIdAsync(int id)
    {
        return await _context.Repuestos.FindAsync(id);
    }

    public async Task<IEnumerable<Repuesto>> GetAllAsync()
    {
        return await _context.Repuestos.ToListAsync();
    }

    public async Task<IEnumerable<Repuesto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Repuestos
            .OrderBy(r => r.Descripcion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Repuestos.CountAsync();
    }

    public async Task<Repuesto?> GetByCodigoAsync(string codigo)
    {
        return await _context.Repuestos
            .FirstOrDefaultAsync(r => r.Codigo == codigo);
    }

    public async Task<IEnumerable<Repuesto>> GetByCategoriaAsync(string categoria)
    {
        return await _context.Repuestos
            .Where(r => r.Categoria == categoria)
            .ToListAsync();
    }

    public async Task<IEnumerable<Repuesto>> GetBajoStockAsync()
    {
        return await _context.Repuestos
            .Where(r => r.CantidadStock <= r.StockMinimo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Repuesto>> SearchByDescripcionAsync(string descripcion)
    {
        return await _context.Repuestos
            .Where(r => r.Descripcion.Contains(descripcion))
            .ToListAsync();
    }

    public async Task<bool> ExisteCodigoAsync(string codigo)
    {
        return await _context.Repuestos
            .AnyAsync(r => r.Codigo == codigo);
    }

    public async Task<bool> TieneStockDisponibleAsync(int repuestoId, int cantidadRequerida)
    {
        var repuesto = await _context.Repuestos.FindAsync(repuestoId);
        return repuesto != null && repuesto.CantidadStock >= cantidadRequerida;
    }

    public async Task ActualizarStockAsync(int repuestoId, int cantidad)
    {
        var repuesto = await _context.Repuestos.FindAsync(repuestoId);
        if (repuesto != null)
        {
            repuesto.CantidadStock += cantidad;
            _context.Repuestos.Update(repuesto);
        }
    }

    public async Task AddAsync(Repuesto repuesto)
    {
        await _context.Repuestos.AddAsync(repuesto);
    }

    public void Update(Repuesto repuesto)
    {
        _context.Repuestos.Update(repuesto);
    }

    public void Delete(Repuesto repuesto)
    {
        _context.Repuestos.Remove(repuesto);
    }
}