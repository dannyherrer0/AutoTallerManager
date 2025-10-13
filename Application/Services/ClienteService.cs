using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<(IEnumerable<ClienteDto> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<ClienteDto?> GetByIdAsync(int clienteId);
    Task<ClienteDto> CreateAsync(CreateClienteDto createDto);
    Task<bool> UpdateAsync(int clienteId, UpdateClienteDto updateDto);
    Task<bool> DeleteAsync(int clienteId);
    Task<IEnumerable<ClienteDto>> SearchByNombreAsync(string nombre);
}

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClienteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<(IEnumerable<ClienteDto> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var clientes = await _unitOfWork.Clientes.GetPagedAsync(pageNumber, pageSize);
        var totalCount = await _unitOfWork.Clientes.GetTotalCountAsync();
        
        var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        return (clientesDto, totalCount);
    }

    public async Task<ClienteDto?> GetByIdAsync(int clienteId)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId);
        return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto createDto)
    {
        if (await _unitOfWork.Clientes.ExisteCorreoAsync(createDto.Correo!))
            throw new InvalidOperationException("El correo ya está registrado");

        var cliente = _mapper.Map<Cliente>(createDto);
        cliente.FechaRegistro = DateTime.UtcNow;
        
        await _unitOfWork.Clientes.AddAsync(cliente);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<bool> UpdateAsync(int clienteId, UpdateClienteDto updateDto)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId);
        if (cliente == null)
            return false;

        var clienteConCorreo = await _unitOfWork.Clientes.GetByCorreoAsync(updateDto.Correo!);
        if (clienteConCorreo != null && clienteConCorreo.ClienteId != clienteId)
            throw new InvalidOperationException("El correo ya está registrado por otro cliente");

        _mapper.Map(updateDto, cliente);
        _unitOfWork.Clientes.Update(cliente);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int clienteId)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId);
        if (cliente == null)
            return false;

        _unitOfWork.Clientes.Delete(cliente);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ClienteDto>> SearchByNombreAsync(string nombre)
    {
        var clientes = await _unitOfWork.Clientes.SearchByNombreAsync(nombre);
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }
}