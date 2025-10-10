using Domain.Entities;

namespace Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(int id);
    Task<Cliente?> GetByIdWithVehiculosAsync(int id);
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<IEnumerable<Cliente>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Cliente?> GetByCorreoAsync(string correo);
    Task<IEnumerable<Cliente>> SearchByNombreAsync(string nombre);
    Task<bool> ExisteCorreoAsync(string correo);
    Task AddAsync(Cliente cliente);
    void Update(Cliente cliente);
    void Delete(Cliente cliente);
}