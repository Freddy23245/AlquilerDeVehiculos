using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Tipo
{
    public int IdTipo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string DescripcionAmpliada { get; set; } = null!;

    public decimal CostoAlquiler { get; set; }

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
