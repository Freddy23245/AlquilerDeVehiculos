using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Metodo
{
    public int IdMetodo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
