using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class PrecioVehiculo
{
    public int IdPrecioVehiculo { get; set; }

    public int IdVehiculo { get; set; }

    public decimal PrecioPorDia { get; set; }

    public decimal? PrecioPorHora { get; set; }

    public DateOnly FechaVigenciaDesde { get; set; }

    public DateOnly? FechaVigenciaHasta { get; set; }

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
