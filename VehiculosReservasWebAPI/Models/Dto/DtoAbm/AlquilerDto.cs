namespace VehiculosReservasWebAPI.Models.Dto.DtoAbm
{
    public class AlquilerDto
    { //Cambio AlquilerDto Test
        public int IdAlquiler { get; set; }

        public int IdEmpleado { get; set; }

        public int IdCliente { get; set; }

        public int IdVehiculo { get; set; }
        public int? IdOpcionAlquiler { get; set; }
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public decimal Costo { get; set; }
    }
}
