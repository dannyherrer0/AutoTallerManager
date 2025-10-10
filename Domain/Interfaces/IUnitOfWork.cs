namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositorios específicos
    IClienteRepository Clientes { get; }
    IVehiculoRepository Vehiculos { get; }
    IOrdenServicioRepository OrdenesServicio { get; }
    IRepuestoRepository Repuestos { get; }
    IUsuarioRepository Usuarios { get; }
    IFacturaRepository Facturas { get; }
    
    // Métodos de transacción
    Task<int> CommitAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
