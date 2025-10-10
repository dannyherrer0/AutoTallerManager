using Domain.Entities;

namespace Domain.Interfaces;

public interface IVehiculoRepository
{
    Task<Vehiculo?> GetByIdAsync(int id);
    Task<Vehiculo?> GetByIdWithClienteAsync(int id);
    Task<IEnumerable<Vehiculo>> GetAllAsync();
    Task<IEnumerable<Vehiculo>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Vehiculo?> GetByVINAsync(string vin);
    Task<IEnumerable<Vehiculo>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<Vehiculo>> SearchByMarcaModeloAsync(string searchTerm);
    Task<bool> ExisteVINAsync(string vin);
    Task<bool> TieneOrdenesActivasAsync(int vehiculoId);
    Task AddAsync(Vehiculo vehiculo);
    void Update(Vehiculo vehiculo);
    void Delete(Vehiculo vehiculo);
}