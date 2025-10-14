using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepuestoRepository
{
    Task<Repuesto?> GetByIdAsync(int id);
    Task<IEnumerable<Repuesto>> GetAllAsync();
    Task<IEnumerable<Repuesto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Repuesto?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<Repuesto>> GetByCategoriaAsync(string categoria);
    Task<IEnumerable<Repuesto>> GetBajoStockAsync();
    Task<IEnumerable<Repuesto>> SearchByDescripcionAsync(string descripcion);
    Task<bool> ExisteCodigoAsync(string codigo);
    Task<bool> TieneStockDisponibleAsync(int repuestoId, int cantidadRequerida);
    Task ActualizarStockAsync(int repuestoId, int cantidad);
    Task AddAsync(Repuesto repuesto);
    void Update(Repuesto repuesto);
    void Delete(Repuesto repuesto);
}
