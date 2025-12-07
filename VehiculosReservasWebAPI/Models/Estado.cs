using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
