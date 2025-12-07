namespace VehiculosReservasWebAPI.Models.Dto.DtoViews
{
    public class AlquilerListadoDto
    {
        public int IdAlquiler { get; set; }
        public string NombreCliente { get; set; }
        public string NombreEmpleado { get; set; }
        public string VehiculoDescripcion { get; set; } // Marca + Modelo + Patente
        public string OpcionAlquiler { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public decimal PrecioDia { get; set; }
        public decimal PrecioHora { get; set; }
        public decimal Costo { get; set; }
        public string Estado { get; set; }
    }

}
