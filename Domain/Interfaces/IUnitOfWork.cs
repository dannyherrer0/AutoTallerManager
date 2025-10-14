namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClienteRepository Clientes { get; }
    IOrdenServicioRepository OrdenesServicio { get; }
    IFacturaRepository Facturas { get; }
    IVehiculoRepository Vehiculos { get; }
    IRepuestoRepository Repuestos { get; }
    IUsuarioRepository Usuarios { get; }


    Task<int> CommitAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
