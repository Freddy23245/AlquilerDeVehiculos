namespace VehiculosReservasWebAPI.Models.Dto.DtoAbm
{
    public class PagoDto
    {
        public int IdPago { get; set; }

        public int IdEmpleado { get; set; }

        public int IdAlquiler { get; set; }

        public int IdMetodo { get; set; }

        public DateTime? Fecha { get; set; }

        public decimal Importe { get; set; }
    }
}
