using VehiculosReservasWebAPI.Models.Dto.DtoAbm;

namespace VehiculosReservasWebAPI.Services.IService
{
    public interface IServiceAlquiler
    {
        Task CrearAlquiler(AlquilerDto dto);
    }
}
