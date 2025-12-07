using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class OpcionAlquiler
{
    public int IdOpcionAlquiler { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Habilitado { get; set; }

    public virtual ICollection<Alquiler> Alquilers { get; set; } = new List<Alquiler>();
}
