using System.ComponentModel.DataAnnotations;

namespace VehiculosReservasWebAPI.Models.Dto.DtoViews
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "Se requiere que ingese el nombre de usuario.")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Se requiere que ingese la password.")]
        public string Password { get; set; }
    }
}
