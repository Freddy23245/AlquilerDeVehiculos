namespace VehiculosReservasWebAPI.Models.Dto.DtoAbm
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public bool? Habilitado { get; set; }
    }
}
