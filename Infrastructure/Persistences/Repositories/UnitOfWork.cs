using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AutoTallerDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AutoTallerDbContext context)
    {
        _context = context;
        
       
        Clientes = new ClienteRepository(_context);
        Vehiculos = new VehiculoRepository(_context);
        OrdenesServicio = new OrdenServicioRepository(_context);
        Repuestos = new RepuestoRepository(_context);
        Usuarios = new UsuarioRepository(_context);
        Facturas = new FacturaRepository(_context);
    }

    public IClienteRepository Clientes { get; }
    public IVehiculoRepository Vehiculos { get; }
    public IOrdenServicioRepository OrdenesServicio { get; }
    public IRepuestoRepository Repuestos { get; }
    public IUsuarioRepository Usuarios { get; }
    public IFacturaRepository Facturas { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}