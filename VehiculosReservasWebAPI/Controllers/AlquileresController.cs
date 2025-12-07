using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        private readonly IService<Alquiler> _AlquilerService;
        private readonly IAlquilerRepository _AlquilerRepository2;
        private readonly IVehiculoRepository _VehiculoRepository;
        private readonly IMapper _mapper;
        public AlquileresController(IService<Alquiler> AlquilerService, IMapper mapper, IAlquilerRepository AlquilerRepository2, IVehiculoRepository vehiculoRepository)
        {
            _AlquilerService = AlquilerService;
            _mapper = mapper;
            _AlquilerRepository2 = AlquilerRepository2;
            _VehiculoRepository = vehiculoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerListadoAlquileres()
        {
            var clientes = await _AlquilerRepository2.ListadoAlquiler();
            var dto = _mapper.Map<List<AlquilerListadoDto>>(clientes);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAlquiler(AlquilerDto NuevoAlquiler)
        {
            try
            {
                var modeloAlq = _mapper.Map<Alquiler>(NuevoAlquiler);

                var resultado = await _AlquilerRepository2.TraerPrecioSegunTipoAlquiler((int)NuevoAlquiler.IdOpcionAlquiler, NuevoAlquiler.IdVehiculo, NuevoAlquiler.FechaInicio, NuevoAlquiler.FechaFin); // cantidad dia (Fecha Inicio + FechaFin ) * precio
                modeloAlq.Costo = resultado;
                modeloAlq.FechaEntrega = null;

                var seSuperpone = _AlquilerRepository2.ExisteSuperposicion(NuevoAlquiler.IdVehiculo, NuevoAlquiler.FechaInicio, NuevoAlquiler.FechaFin);
                if (seSuperpone)
                    return BadRequest("El vehículo ya está alquilado en ese período.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (seSuperpone)
                    await _VehiculoRepository.ObtenerEstadoVehiculo(NuevoAlquiler.IdVehiculo);
                await _AlquilerService.Agregar(modeloAlq);
                await _VehiculoRepository.ActualizarEstadoVehiculo(NuevoAlquiler.IdVehiculo);

                return Ok("Alquiler Agregado correctamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {mensaje = ex.Message});
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditarAlquiler(AlquilerDto ModAlquiler)
        {
            try
            {
                // Obtenemos el alquiler original de la base de datos
                var alquilerOriginal = await _AlquilerService.ObtenerPorId(ModAlquiler.IdAlquiler);
                if (alquilerOriginal == null)
                    return NotFound("El alquiler no existe.");
                var snapshot = new Alquiler
                {
                    IdVehiculo = alquilerOriginal.IdVehiculo,
                    FechaInicio = alquilerOriginal.FechaInicio,
                    FechaFin = alquilerOriginal.FechaFin
                };

                bool huboCambios =
                snapshot.IdVehiculo != ModAlquiler.IdVehiculo || snapshot.FechaInicio != ModAlquiler.FechaInicio || snapshot.FechaFin != ModAlquiler.FechaFin;
                // Verificamos superposición solo si cambian fechas o vehículo
                bool seSuperpone = false;
                if (huboCambios)
                {
                    seSuperpone = _AlquilerRepository2.ExisteSuperposicionEditar(ModAlquiler.IdVehiculo,ModAlquiler.FechaInicio,ModAlquiler.FechaFin,ModAlquiler.IdAlquiler);
                }

                if (seSuperpone)
                    return BadRequest("El vehículo ya está alquilado en ese período.");
                TimeSpan diferencia = ModAlquiler.FechaFin - ModAlquiler.FechaInicio;
                double totalHoras = diferencia.TotalHours;
                if (totalHoras < 24)
                    ModAlquiler.IdOpcionAlquiler = 2;//Por Hora
                else
                    ModAlquiler.IdOpcionAlquiler = 1;//Por Dia
                // Calculamos el precio
                var resultado = await _AlquilerRepository2.TraerPrecioSegunTipoAlquiler(
                    (int)ModAlquiler.IdOpcionAlquiler,ModAlquiler.IdVehiculo,ModAlquiler.FechaInicio,ModAlquiler.FechaFin);
                ModAlquiler.Costo = resultado;
                ModAlquiler.FechaEntrega = alquilerOriginal.FechaEntrega; 

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Mapear DTO a entidad
                var modeloAlq = _mapper.Map<Alquiler>(ModAlquiler);
                modeloAlq.Finalizado = alquilerOriginal.Finalizado;
                // Guardamos los cambios del alquiler
                await _AlquilerService.Editar(modeloAlq);

                // Actualizamos el estado del vehículo solo si hubo cambios en vehículo o fechas
                if (huboCambios)
                {
                    // Si cambió de vehículo → actualizar el anterior
                    if (snapshot.IdVehiculo != ModAlquiler.IdVehiculo)
                        await _VehiculoRepository.ActualizarEstadoVehiculo(snapshot.IdVehiculo);
                        // Actualizar el nuevo
                    await _VehiculoRepository.ActualizarEstadoVehiculo(ModAlquiler.IdVehiculo);
                }
                else
                {
                    // Mismo vehículo, pero recalculamos fechas
                    await _VehiculoRepository.ActualizarEstadoVehiculo(ModAlquiler.IdVehiculo);
                }

                return Ok("Alquiler Modificado correctamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> EliminarAlquiler(int id)
        {

            var alquilerOriginal = await _AlquilerService.ObtenerPorId(id);
            bool tieneReservaFutura = _AlquilerRepository2.VehiculoTieneReservaFutura(alquilerOriginal.IdVehiculo,alquilerOriginal.FechaFin,alquilerOriginal.IdAlquiler);

            await _AlquilerService.Eliminar(id);
            int valor = 0;
            valor = alquilerOriginal.IdOpcionAlquiler ?? 0;
            alquilerOriginal.CostoRetraso = await _AlquilerRepository2.TraerRetraso(alquilerOriginal.IdVehiculo, valor, alquilerOriginal.FechaFin, alquilerOriginal.FechaEntrega);
            await _AlquilerService.Editar(alquilerOriginal);
            if (!tieneReservaFutura)
            {
                // No tiene reserva futura → vehículo disponible
                await _VehiculoRepository.ActualizarEstadoVehiculo(alquilerOriginal.IdVehiculo);
            }
            else
            {
                // Tiene reserva pendiente → marcar como "Reservado", no "Disponible"
                await _VehiculoRepository.ActualizarEstadoVehiculo(alquilerOriginal.IdVehiculo);
            }

            return Ok("Alquiler Finalizado Correctamente!");
        }
    }
}
