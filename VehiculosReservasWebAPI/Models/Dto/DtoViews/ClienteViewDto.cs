namespace VehiculosReservasWebAPI.Models.Dto.DtoViews
{
    public class ClienteViewDto
    {
        public string Nombre { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public bool? Habilitado { get; set; }
    }
}
