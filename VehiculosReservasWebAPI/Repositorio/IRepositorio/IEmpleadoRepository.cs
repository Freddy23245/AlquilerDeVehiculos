using VehiculosReservasWebAPI.Models.Dto.DtoViews;

namespace VehiculosReservasWebAPI.Repositorio.IRepositorio
{
    public interface IEmpleadoRepository
    {
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuario);
    }
}
