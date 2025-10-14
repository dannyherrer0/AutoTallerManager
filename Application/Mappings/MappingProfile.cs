using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 🧩 CLIENTE
        CreateMap<Cliente, ClienteDto>();
        CreateMap<CreateClienteDto, Cliente>();
        CreateMap<UpdateClienteDto, Cliente>();

        // 🧩 VEHICULO
        CreateMap<Vehiculo, VehiculoDto>()
            .ForMember(dest => dest.NombreCliente, 
                opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : null))
            .ForMember(dest => dest.NombreMarca, 
                opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Nombre : null));
        CreateMap<CreateVehiculoDto, Vehiculo>();
        CreateMap<UpdateVehiculoDto, Vehiculo>();

        // 🧩 MARCA
        CreateMap<Brand, BrandDto>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();

        // 🧩 REPUESTO
        CreateMap<Repuesto, RepuestoDto>();
        CreateMap<CreateRepuestoDto, Repuesto>();
        CreateMap<UpdateRepuestoDto, Repuesto>();

        // 🧩 DETALLE ORDEN
        CreateMap<DetalleOrden, DetalleOrdenDto>()
            .ForMember(dest => dest.CodigoRepuesto, 
                opt => opt.MapFrom(src => src.Repuesto != null ? src.Repuesto.Codigo : null))
            .ForMember(dest => dest.DescripcionRepuesto, 
                opt => opt.MapFrom(src => src.Repuesto != null ? src.Repuesto.Descripcion : null))
            .ForMember(dest => dest.Subtotal, 
                opt => opt.MapFrom(src => src.Cantidad * src.PrecioUnitario));
        CreateMap<CreateDetalleOrdenDto, DetalleOrden>()
            .ForMember(dest => dest.Subtotal, 
                opt => opt.MapFrom(src => src.Cantidad * src.PrecioUnitario));

        // 🧩 ORDEN DE SERVICIO
        CreateMap<OrdenServicio, OrdenServicioDto>()
            .ForMember(dest => dest.ModeloVehiculo, 
                opt => opt.MapFrom(src => src.Vehiculo.Modelo))
            .ForMember(dest => dest.NombreCliente, 
                opt => opt.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.Estado, 
                opt => opt.MapFrom(src => src.Estado.ToString()))
            .ForMember(dest => dest.FechaEntrega, 
                opt => opt.MapFrom(src => src.FechaEntregaReal))
            .ForMember(dest => dest.Detalles, 
                opt => opt.MapFrom(src => src.DetallesOrden))
            .ForMember(dest => dest.Total, 
                opt => opt.MapFrom(src => src.CostoManoObra + 
                    (src.DetallesOrden != null ? src.DetallesOrden.Sum(d => d.Subtotal) : 0)));

        // 🧩 FACTURA
        CreateMap<Factura, FacturaDto>()
            .ForMember(dest => dest.NombreCliente, 
                opt => opt.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.MontoTotal, 
                opt => opt.MapFrom(src => src.SubtotalRepuestos + src.SubtotalManoObra + src.IVA - src.Descuento));
        CreateMap<CreateFacturaDto, Factura>()
            .ForMember(dest => dest.Estado, 
                opt => opt.MapFrom(src => EstadoFactura.Anulada))
            .ForMember(dest => dest.MontoTotal, 
                opt => opt.MapFrom(src => src.SubtotalRepuestos + src.SubtotalManoObra + src.IVA - src.Descuento));
        CreateMap<UpdateFacturaDto, Factura>();

        // 🧩 USUARIO / AUTENTICACIÓN
        CreateMap<Usuario, UsuarioDto>();
        CreateMap<RegisterDto, Usuario>()
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo));

        CreateMap<LoginDto, Usuario>();
        CreateMap<Usuario, AuthResponseDto>()
            .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => new UsuarioDto
            {
                Id = src.Id,
                Nombre = src.Nombre,
                Correo = src.Correo,
                Rol = src.Rol,
                Activo = src.Activo
            }));
    }
}