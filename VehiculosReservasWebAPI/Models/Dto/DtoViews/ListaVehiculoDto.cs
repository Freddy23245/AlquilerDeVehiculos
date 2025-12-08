namespace VehiculosReservasWebAPI.Models.Dto.DtoViews
{
    public class ListaVehiculoDto
    {
        public int IdVehiculo { get; set; }
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public int Ano { get; set; }
        public string Color { get; set; }
        public int Kilometraje { get; set; }
        public string Transmision { get; set; }
        public string Combustible { get; set; }
        public int  CapacidadPasajeros { get; set; }
        public decimal PrecioCompra { get; set; }
        public string Observaciones { get; set; }
        public DateOnly FechaAlta { get; set; }
        public bool Habilitado { get; set; }
    }
}
