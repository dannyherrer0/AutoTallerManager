using Domain.Entities;

namespace Domain.Interfaces;

public interface IFacturaRepository
{
    Task<Factura?> GetByIdAsync(int id);
    Task<Factura?> GetByIdWithOrdenAsync(int id);
    Task<IEnumerable<Factura>> GetAllAsync();
    Task<IEnumerable<Factura>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Factura?> GetByNumeroFacturaAsync(string numeroFactura);
    Task<Factura?> GetByOrdenServicioIdAsync(int ordenServicioId);
    Task<IEnumerable<Factura>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Factura>> GetByClienteIdAsync(int clienteId);
    Task<decimal> GetTotalFacturadoEnRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<string> GenerarNumeroFacturaAsync();
    Task AddAsync(Factura factura);
    void Update(Factura factura);
    void Delete(Factura factura);
}