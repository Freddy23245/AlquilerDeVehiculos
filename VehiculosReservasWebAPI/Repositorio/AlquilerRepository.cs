using Microsoft.EntityFrameworkCore;
using System.Security;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;

namespace VehiculosReservasWebAPI.Repositorio
{
    public class AlquilerRepository : IAlquilerRepository
    {
        private readonly ReservasCocheraContext _context;
        public AlquilerRepository(ReservasCocheraContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AlquilerListadoDto>> ListadoAlquiler()
        {
            var listado = await _context.Alquilers
                .Select(a => new AlquilerListadoDto
                {
                    IdAlquiler = a.IdAlquiler,
                    NombreCliente = a.IdClienteNavigation.Nombre,
                    NombreEmpleado = a.IdEmpleadoNavigation.Nombre,
                    VehiculoDescripcion = a.IdVehiculoNavigation.IdMarcaNavigation.Nombre + " - " + a.IdVehiculoNavigation.IdModeloNavigation.Nombre + " - " + a.IdVehiculoNavigation.Patente,
                    OpcionAlquiler = a.IdOpcionAlquilerNavigation.Descripcion,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    FechaEntrega = a.FechaEntrega,
                    PrecioDia = a.IdVehiculoNavigation.PrecioVehiculos.Select(x => x.PrecioPorDia).FirstOrDefault(),
                    PrecioHora = (decimal)a.IdVehiculoNavigation.PrecioVehiculos.Select(x => x.PrecioPorHora).FirstOrDefault(),
                    Costo = a.Costo,
                    Estado = a.Finalizado == true ? "Finalizado" : "En Curso"

                }).ToListAsync();

            return listado;
        }

        public async Task<decimal> TraerPrecioSegunTipoAlquiler(int idTipoAlquiler, int idVehiculo, DateTime fechaInicio, DateTime fechaFin)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            decimal resultado = 0;
            // Traemos de una sola vez el tipo de alquiler y el precio vigente del vehículo
            var tipoAlquiler = await _context.OpcionAlquilers
                .Where(x => x.IdOpcionAlquiler == idTipoAlquiler && x.Habilitado == true)
                .FirstOrDefaultAsync();

            if (tipoAlquiler == null)
                throw new ArgumentException("Opción de alquiler inválida o deshabilitada");

            var precioAlquiler = await _context.PrecioVehiculos
                .Where(x => x.IdVehiculo == idVehiculo
                            && x.FechaVigenciaDesde <= hoy
                            && (x.FechaVigenciaHasta == null || x.FechaVigenciaHasta >= hoy))
                .FirstOrDefaultAsync();

            if (precioAlquiler == null)
                throw new InvalidOperationException("No existe un precio vigente para este vehículo");

            if (tipoAlquiler.IdOpcionAlquiler == 1)//dia
              return  resultado = CalcularPrecioPorDia(precioAlquiler.PrecioPorDia, fechaInicio, fechaFin);
            if (tipoAlquiler.IdOpcionAlquiler == 2)//hora
              return  resultado = CalcularPrecioPorHora((decimal)precioAlquiler.PrecioPorHora, fechaInicio, fechaFin);

            return resultado;
        }

        public bool ExisteSuperposicion(int idVehiculo, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            return _context.Alquilers.Any(a =>
                a.IdVehiculo == idVehiculo &&
                a.FechaEntrega == null && a.Finalizado == false &&
                (
                    // La nueva fecha de inicio está dentro de un alquiler existente
                    (nuevaFechaInicio >= a.FechaInicio && nuevaFechaInicio < a.FechaFin) ||

                    // La nueva fecha de fin está dentro de un alquiler existente
                    (nuevaFechaFin > a.FechaInicio && nuevaFechaFin <= a.FechaFin) ||

                    // La nueva fecha engloba un alquiler existente
                    (nuevaFechaInicio <= a.FechaInicio && nuevaFechaFin >= a.FechaFin)
                )
            );
        }

        public bool ExisteSuperposicionEditar(int idVehiculo, DateTime nuevaFechaInicio, DateTime nuevaFechaFin, int IdAlquiler)
        {
            return _context.Alquilers.Any(a =>
                a.IdVehiculo == idVehiculo &&
                a.FechaEntrega == null && a.Finalizado == false && a.IdAlquiler != IdAlquiler &&
                (
                    // La nueva fecha de inicio está dentro de un alquiler existente
                    (nuevaFechaInicio >= a.FechaInicio && nuevaFechaInicio < a.FechaFin) ||

                    // La nueva fecha de fin está dentro de un alquiler existente
                    (nuevaFechaFin > a.FechaInicio && nuevaFechaFin <= a.FechaFin) ||

                    // La nueva fecha engloba un alquiler existente
                    (nuevaFechaInicio <= a.FechaInicio && nuevaFechaFin >= a.FechaFin)
                )
            );
        }

        public bool VehiculoTieneReservaFutura(int idVehiculo, DateTime fechaActual, int idAlquilerActual)
        {
            return _context.Alquilers.Any(a =>
                a.IdVehiculo == idVehiculo &&
                a.IdAlquiler != idAlquilerActual &&     // excluimos el alquiler actual
                a.Finalizado == false &&                // alquiler pendiente
                a.FechaEntrega == null &&              // aún no fue devuelto
                a.FechaInicio > fechaActual            // comienza después de ahora
            );
        }

        public int CalcularDiasRetraso(DateTime fechaFin, DateTime? fechaEntrega)
        {
            if (!fechaEntrega.HasValue)
                return 0; // No se entregó aún, no se puede calcular

            int diasRetraso = (int)Math.Ceiling((fechaEntrega.Value - fechaFin).TotalDays);
            //return diasRetraso > 0 ? diasRetraso : 0;
            return diasRetraso;
        }

        public async Task<decimal?> TraerRetraso(int idVehiculo, int idTipoAlquiler, DateTime fechaFin, DateTime? fechaEntrega)
        {
            decimal recargo = 0;
            if (!fechaEntrega.HasValue)
                return 0m;

            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // Traemos el tipo de alquiler
            var tipoAlquiler = await _context.OpcionAlquilers
                .FirstOrDefaultAsync(x => x.IdOpcionAlquiler == idTipoAlquiler && x.Habilitado == true);

            if (tipoAlquiler == null)
                throw new ArgumentException("Opción de alquiler inválida o deshabilitada");

            // Traemos el precio vigente del vehículo
            var precioAlquiler = await _context.PrecioVehiculos
                .FirstOrDefaultAsync(x => x.IdVehiculo == idVehiculo
                                          && x.FechaVigenciaDesde <= hoy
                                          && (x.FechaVigenciaHasta == null || x.FechaVigenciaHasta >= hoy));

            if (precioAlquiler == null)
                throw new InvalidOperationException("No existe un precio vigente para este vehículo");
           
            if (tipoAlquiler.IdOpcionAlquiler == 1)//dia
                return recargo = CalcularPrecioPorDia(precioAlquiler.PrecioPorDia, fechaFin, (DateTime)fechaEntrega);
            if (tipoAlquiler.IdOpcionAlquiler == 2)//hora
                return recargo = CalcularPrecioPorHora((decimal)precioAlquiler.PrecioPorHora, fechaFin, (DateTime)fechaEntrega);

            return recargo;//falta verificar la sobrecarga del dato
        }

        public decimal CalcularPrecioPorDia(decimal precioPorDia, DateTime fechaInicio, DateTime fechaFin)
        {
            // Calculamos la diferencia de días exacta
            TimeSpan diferencia = fechaFin - fechaInicio;

            // Redondeamos hacia arriba para cobrar días completos
            int cantidadDias = (int)Math.Ceiling(diferencia.TotalDays);

            // Calculamos el precio total
            decimal precioTotal = precioPorDia * cantidadDias;
            //if (precioTotal < precioPorDia)
            //    precioTotal = 0;

            return Math.Round(precioTotal, 2);
        }


        public decimal CalcularPrecioPorHora(decimal precioPorHora, DateTime fechaInicio, DateTime fechaFin)
        {

            // Calculamos la diferencia exacta en horas
            TimeSpan diferencia = fechaFin - fechaInicio;
            double cantidadHoras = diferencia.TotalHours; // incluye minutos y segundos

            // Calculamos el precio total (hora exacta)
            decimal precioTotal = precioPorHora * (decimal)cantidadHoras;

            //if (precioTotal < precioPorHora)
            //    precioTotal = 0;

            return Math.Round(precioTotal, 2);
        }

    }
}
