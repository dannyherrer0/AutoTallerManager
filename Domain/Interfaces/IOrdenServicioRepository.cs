using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrdenServicioRepository
{
    Task<OrdenServicio?> GetByIdAsync(int id);
    Task<OrdenServicio?> GetByIdWithDetallesAsync(int id);
    Task<OrdenServicio?> GetByIdCompleteAsync(int id); // Con Vehículo, Cliente, Mecánico, Detalles
    Task<IEnumerable<OrdenServicio>> GetAllAsync();
    Task<IEnumerable<OrdenServicio>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<OrdenServicio>> GetByEstadoAsync(string estado);
    Task<IEnumerable<OrdenServicio>> GetByMecanicoIdAsync(int mecanicoId);
    Task<IEnumerable<OrdenServicio>> GetByVehiculoIdAsync(int vehiculoId);
    Task<IEnumerable<OrdenServicio>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<OrdenServicio>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<bool> VehiculoTieneOrdenActivaAsync(int vehiculoId);
    Task<int> GetOrdenesActivasPorMecanicoAsync(int mecanicoId);
    Task AddAsync(OrdenServicio orden);
    void Update(OrdenServicio orden);
    void Delete(OrdenServicio orden);
}
