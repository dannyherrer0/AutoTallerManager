using Domain.Entities;

namespace Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(int id);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<IEnumerable<Usuario>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<IEnumerable<Usuario>> GetByRolAsync(string rol);
    Task<IEnumerable<Usuario>> GetMecanicosDisponiblesAsync();
    Task<bool> ExisteCorreoAsync(string correo);
    Task<bool> ValidarCredencialesAsync(string correo, string passwordHash);
    Task AddAsync(Usuario usuario);
    void Update(Usuario usuario);
    void Delete(Usuario usuario);
    Task<Usuario> GetByCorreoAsync(object correo);
}
