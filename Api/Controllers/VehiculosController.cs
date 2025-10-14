using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiculosController : ControllerBase
{
    private readonly IVehiculoService _vehiculoService;

    public VehiculosController(IVehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }

    /// <summary>
    /// Obtener todos los vehículos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetAll()
    {
        var vehiculos = await _vehiculoService.GetAllAsync();
        return Ok(vehiculos);
    }

    /// <summary>
    /// Obtener vehículos paginados
    /// </summary>
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetPaged(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var (vehiculos, totalCount) = await _vehiculoService.GetPagedAsync(pageNumber, pageSize);
        
        Response.Headers.Add("X-Total-Count", totalCount.ToString());
        Response.Headers.Add("X-Page-Number", pageNumber.ToString());
        Response.Headers.Add("X-Page-Size", pageSize.ToString());
        
        return Ok(vehiculos);
    }

    /// <summary>
    /// Obtener vehículo por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehiculoDto>> GetById(int id)
    {
        var vehiculo = await _vehiculoService.GetByIdAsync(id);
        
        if (vehiculo == null)
            return NotFound(new { message = "Vehículo no encontrado" });

        return Ok(vehiculo);
    }

    /// <summary>
    /// Obtener vehículos por cliente
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetByCliente(int clienteId)
    {
        var vehiculos = await _vehiculoService.GetByClienteIdAsync(clienteId);
        return Ok(vehiculos);
    }

    /// <summary>
    /// Crear nuevo vehículo
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Recepcionista")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VehiculoDto>> Create([FromBody] CreateVehiculoDto createDto)
    {
        try
        {
            var vehiculo = await _vehiculoService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = vehiculo.VehiculoId }, vehiculo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualizar vehículo
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Recepcionista")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateVehiculoDto updateDto)
    {
        var result = await _vehiculoService.UpdateAsync(id, updateDto);
        
        if (!result)
            return NotFound(new { message = "Vehículo no encontrado" });

        return Ok(new { message = "Vehículo actualizado correctamente" });
    }

    /// <summary>
    /// Eliminar vehículo
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _vehiculoService.DeleteAsync(id);
            
            if (!result)
                return NotFound(new { message = "Vehículo no encontrado" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}