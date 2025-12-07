namespace VehiculosReservasWebAPI.Models.Dto.DtoViews
{
    public class TipoDto
    {
        public int IdTipo { get; set; }

        public string Descripcion { get; set; } = null!;

        public string DescripcionAmpliada { get; set; } = null!;

        public decimal CostoAlquiler { get; set; }
    }
}
