using AutoMapper;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;

namespace VehiculosReservasWebAPI.ReservasMappers
{
    public class VehiculosMappers : Profile
    {
        public VehiculosMappers()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Alquiler, AlquilerDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Estado, EstadoDto>().ReverseMap();
            CreateMap<Marca, MarcaDto>().ReverseMap();
            CreateMap<Metodo, MetodoDto>().ReverseMap();
            CreateMap<Modelo, ModeloDto>().ReverseMap();
            CreateMap<Pago, PagoDto>().ReverseMap();
            CreateMap<Tipo, TipoDto>().ReverseMap();
            CreateMap<Vehiculo, VehiculoDto>().ReverseMap();
        }
    }
}
