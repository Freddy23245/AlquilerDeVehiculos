using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Alquiler
{
    public int IdAlquiler { get; set; }

    public int IdEmpleado { get; set; }

    public int IdCliente { get; set; }

    public int IdVehiculo { get; set; }

    public int? IdOpcionAlquiler { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal Costo { get; set; }

    public decimal? CostoRetraso { get; set; }

    public bool? Finalizado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual OpcionAlquiler? IdOpcionAlquilerNavigation { get; set; }

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
