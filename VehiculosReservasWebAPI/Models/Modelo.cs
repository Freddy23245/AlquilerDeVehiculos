using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Modelo
{
    public int IdModelo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdMarca { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
