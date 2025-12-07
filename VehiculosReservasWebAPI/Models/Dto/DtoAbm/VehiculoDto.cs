namespace VehiculosReservasWebAPI.Models.Dto.DtoAbm
{
    public class VehiculoDto
    {
        public int IdVehiculo { get; set; }

        public string Patente { get; set; } = null!;

        public int IdMarca { get; set; }

        public int? IdModelo { get; set; }

        public int IdTipo { get; set; }

        public int IdEstado { get; set; }

        public int Año { get; set; }

        public string? Color { get; set; }

        public int? Kilometraje { get; set; }

        public string? Transmision { get; set; }

        public string? Combustible { get; set; }

        public int? CapacidadPasajeros { get; set; }

        public decimal? PrecioCompra { get; set; }

        public string? Observaciones { get; set; }

        public DateOnly? FechaAlta { get; set; }

        public bool? Habilitado { get; set; }
    }
}
