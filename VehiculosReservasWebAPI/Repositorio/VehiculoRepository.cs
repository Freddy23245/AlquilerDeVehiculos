using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Repositorio
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ReservasCocheraContext _context;
        private readonly IService<Vehiculo> _VehiculoService;
        public VehiculoRepository(ReservasCocheraContext context, IService<Vehiculo> VehiculoService)
        {
            _context = context;
            _VehiculoService = VehiculoService;
        }
        public async Task<int> ActualizarEstadoVehiculo(int idVehiculo)
        {
            var hoy = DateTime.Now;

            // Traemos el vehículo junto con sus alquileres activos
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Alquilers)
                .FirstOrDefaultAsync(v => v.IdVehiculo == idVehiculo);

            if (vehiculo == null)
                throw new Exception("Vehículo no encontrado");

            // Buscamos el alquiler activo más reciente
            var alquiler = vehiculo.Alquilers
                .Where(a => a.Finalizado == false)
                .OrderByDescending(a => a.FechaInicio)
                .FirstOrDefault();

            // 1 - Inhabilitado
            if (vehiculo.Habilitado == false)
            {
                return await SetEstado(vehiculo, 6); // Inhabilitado
            }

            // 2 - Disponible (sin alquiler)
            if (alquiler == null)
            {
                return await SetEstado(vehiculo, 1); // Disponible
            }

            // 3 - Reservado
            if (hoy < alquiler.FechaInicio)
            {
                return await SetEstado(vehiculo, 2); // Reservado
            }

            // 4 - Alquilado
            if (hoy >= alquiler.FechaInicio && hoy <= alquiler.FechaFin && alquiler.FechaEntrega == null)
            {
                return await SetEstado(vehiculo, 3); // Alquilado
            }

            // 5 - Retrasado
            if (hoy > alquiler.FechaFin && alquiler.FechaEntrega == null)
            {
                return await SetEstado(vehiculo, 4); // Retrasado
            }

            // 1 - Disponible por defecto
            return await SetEstado(vehiculo, 1);
        }

        public async Task<IEnumerable<ListaVehiculoDto>> ListadoVehiculos()
        {
            var listado = await _context.Vehiculos
                .Select(v => new ListaVehiculoDto
                {
                    IdVehiculo = v.IdVehiculo,
                    Patente = v.Patente,
                    Marca = v.IdMarcaNavigation.Nombre,
                    Modelo = v.IdModeloNavigation.Nombre,
                    Tipo = v.IdTipoNavigation.Descripcion,
                    Estado = v.IdEstadoNavigation.Descripcion,
                    Ano = v.Año,
                    Color = v.Color,
                    Kilometraje = v.Kilometraje ?? 0,
                    Transmision = v.Transmision,
                    Combustible = v.Combustible ?? "",
                    CapacidadPasajeros = v.CapacidadPasajeros ?? 0,
                    PrecioCompra = v.PrecioCompra ?? 0,
                    Observaciones = v.Observaciones,
                    FechaAlta = v.FechaAlta ?? DateOnly.MinValue,
                    Habilitado = v.Habilitado ?? false
                }).ToListAsync();

            return listado;
        }

        public async Task ObtenerEstadoVehiculo(int idVehiculo)
        {
            var estado = await _context.Vehiculos
                .Where(x => x.IdVehiculo == idVehiculo)
                .Select(x => x.IdEstado)
                .FirstOrDefaultAsync();

            switch (estado)
            {
                case 2:
                    throw new Exception("El vehículo está Reservado");
                case 3:
                    throw new Exception("El vehículo está Alquilado");
                case 4:
                    throw new Exception("El vehículo está Retrasado");
                case 5:
                    throw new Exception("El vehículo está en Mantenimiento");
                case 6:
                    throw new Exception("El vehículo está Inhabilitado");
                default:
                    // Estado disponible, no hace nada
                    break;
            }
        }

        private async Task<int> SetEstado(Vehiculo vehiculo, int estado)
        {
            if (vehiculo.IdEstado != estado)
            {
                vehiculo.IdEstado = estado;
                await _context.SaveChangesAsync();
            }
            return estado;
        }

    }
}
