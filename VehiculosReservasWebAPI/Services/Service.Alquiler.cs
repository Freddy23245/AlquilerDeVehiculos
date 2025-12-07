using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Services
{
    public class AlquilerService : IServiceAlquiler
    {
        private readonly IAlquilerRepository _alquilerRepository;       // Validaciones + precio
        private readonly IRepository<Alquiler> _repositoryGenerico;     // Agregar alquiler
        private readonly IVehiculoRepository _vehiculoRepository;        // Cambiar estado
        private readonly IMapper _mapper;

        public AlquilerService(
            IAlquilerRepository alquilerRepository,
            IRepository<Alquiler> repositoryGenerico,
            IVehiculoRepository vehiculoRepository,
            IMapper mapper)
        {
            _alquilerRepository = alquilerRepository;
            _repositoryGenerico = repositoryGenerico;
            _vehiculoRepository = vehiculoRepository;
            _mapper = mapper;
        }
        public async Task CrearAlquiler(AlquilerDto dto)
        {
            if (dto.FechaFin <= dto.FechaInicio)
                throw new Exception ("La fecha de fin debe ser mayor a la fecha de inicio.");

            double horas = (dto.FechaFin - dto.FechaInicio).TotalHours;
            dto.IdOpcionAlquiler = horas < 24 ? 2 : 1;

            var existe = _alquilerRepository.ExisteSuperposicion(dto.IdVehiculo, dto.FechaInicio, dto.FechaFin);
            if (existe)
                throw new Exception("El vehículo ya está alquilado en ese período.");


            // 4️⃣ Mapear DTO → entidad
            var alquiler = _mapper.Map<Alquiler>(dto);

            // 5️⃣ Obtener precio
            alquiler.Costo = await _alquilerRepository.TraerPrecioSegunTipoAlquiler(
                dto.IdOpcionAlquiler.Value,
                dto.IdVehiculo,
                dto.FechaInicio,
                dto.FechaFin
            );

            // 6️⃣ Al crear, la entrega siempre va null
            alquiler.FechaEntrega = null;

            // 7️⃣ Guardar con el repositorio genérico
            await _repositoryGenerico.Agregar(alquiler);

            // 8️⃣ Cambiar estado del vehículo
            await _vehiculoRepository.ActualizarEstadoVehiculo(dto.IdVehiculo);

        }
    }
}
