using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdEmpleado { get; set; }

    public int IdAlquiler { get; set; }

    public int IdMetodo { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Importe { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Metodo IdMetodoNavigation { get; set; } = null!;
}
