

using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;

namespace VehiculosReservasWebAPI.Repositorio.IRepositorio
{
    public interface IAlquilerRepository
    {
        bool ExisteSuperposicion(int idVehiculo, DateTime fechaInicio, DateTime fechaFin);
        bool ExisteSuperposicionEditar(int idVehiculo, DateTime fechaInicio, DateTime fechaFin, int idAlquiler);
        Task<IEnumerable<AlquilerListadoDto>> ListadoAlquiler();
        Task<decimal> TraerPrecioSegunTipoAlquiler(int idOpcionAlquiler, int idVehiculo, DateTime fechaInicio, DateTime fechaFin);
        bool VehiculoTieneReservaFutura(int idVehiculo, DateTime fechaFin, int idAlquiler);
        // Task<decimal> TraerRetraso(int idVehiculo, int idTipoAlquiler, DateTime fechaFin, DateTime? fechaEntrega);
        Task<decimal?> TraerRetraso(int idVehiculo, int idTipoAlquiler, DateTime fechaFin, DateTime? fechaEntrega);
    }
}