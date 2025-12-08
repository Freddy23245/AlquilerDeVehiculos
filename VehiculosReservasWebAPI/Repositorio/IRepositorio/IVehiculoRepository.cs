using VehiculosReservasWebAPI.Models.Dto.DtoViews;

namespace VehiculosReservasWebAPI.Repositorio.IRepositorio
{
    public interface IVehiculoRepository
    {
        Task<int> ActualizarEstadoVehiculo(int idVehiculo);
        Task ObtenerEstadoVehiculo(int idVehiculo);
        Task<IEnumerable<ListaVehiculoDto>> ListadoVehiculos();
    }
}
