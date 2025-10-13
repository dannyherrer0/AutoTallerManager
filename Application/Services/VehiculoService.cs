using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public interface IVehiculoService
{
    Task<IEnumerable<VehiculoDto>> GetAllAsync();
    Task<(IEnumerable<VehiculoDto> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<VehiculoDto?> GetByIdAsync(int vehiculoId);
    Task<IEnumerable<VehiculoDto>> GetByClienteIdAsync(int clienteId);
    Task<VehiculoDto> CreateAsync(CreateVehiculoDto createDto);
    Task<bool> UpdateAsync(int vehiculoId, UpdateVehiculoDto updateDto);
    Task<bool> DeleteAsync(int vehiculoId);
}

public class VehiculoService : IVehiculoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehiculoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehiculoDto>> GetAllAsync()
    {
        var vehiculos = await _unitOfWork.Vehiculos.GetAllAsync();
        return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
    }

    public async Task<(IEnumerable<VehiculoDto> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var vehiculos = await _unitOfWork.Vehiculos.GetPagedAsync(pageNumber, pageSize);
        var totalCount = await _unitOfWork.Vehiculos.GetTotalCountAsync();
        
        var vehiculosDto = _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        return (vehiculosDto, totalCount);
    }

    public async Task<VehiculoDto?> GetByIdAsync(int vehiculoId)
    {
        var vehiculo = await _unitOfWork.Vehiculos.GetByIdWithClienteAsync(vehiculoId);
        return vehiculo == null ? null : _mapper.Map<VehiculoDto>(vehiculo);
    }

    public async Task<IEnumerable<VehiculoDto>> GetByClienteIdAsync(int clienteId)
    {
        var vehiculos = await _unitOfWork.Vehiculos.GetByClienteIdAsync(clienteId);
        return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
    }

    public async Task<VehiculoDto> CreateAsync(CreateVehiculoDto createDto)
    {
        if (await _unitOfWork.Vehiculos.ExisteVINAsync(createDto.VIN))
            throw new InvalidOperationException("El VIN ya está registrado");

        var vehiculo = _mapper.Map<Vehiculo>(createDto);
        vehiculo.FechaRegistro = DateTime.UtcNow;
        
        await _unitOfWork.Vehiculos.AddAsync(vehiculo);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<VehiculoDto>(vehiculo);
    }

    public async Task<bool> UpdateAsync(int vehiculoId, UpdateVehiculoDto updateDto)
    {
        var vehiculo = await _unitOfWork.Vehiculos.GetByIdAsync(vehiculoId);
        if (vehiculo == null)
            return false;

        _mapper.Map(updateDto, vehiculo);
        _unitOfWork.Vehiculos.Update(vehiculo);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int vehiculoId)
    {
        var vehiculo = await _unitOfWork.Vehiculos.GetByIdAsync(vehiculoId);
        if (vehiculo == null)
            return false;

        if (await _unitOfWork.Vehiculos.TieneOrdenesActivasAsync(vehiculoId))
            throw new InvalidOperationException("No se puede eliminar el vehículo porque tiene órdenes activas");

        _unitOfWork.Vehiculos.Delete(vehiculo);
        await _unitOfWork.CommitAsync();

        return true;
    }
}