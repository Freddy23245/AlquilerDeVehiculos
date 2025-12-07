using System.ComponentModel.DataAnnotations;

namespace VehiculosReservasWebAPI.Models.Dto.DtoAbm
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "El cliente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre debe tener no mas de 100 caracteres.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(50, ErrorMessage = "El email debe tener no mas de 50 caracteres.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "El telefono es obligatorio")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(10, ErrorMessage = "El telefono debe tener no mas de 10 caracteres.")]
        public string Telefono { get; set; } = null!;

        public bool? Habilitado { get; set; }
    }
}
