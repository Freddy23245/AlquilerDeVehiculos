using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool? Habilitado { get; set; }

    public virtual ICollection<Alquiler> Alquilers { get; set; } = new List<Alquiler>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
