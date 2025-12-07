using System;
using System.Collections.Generic;

namespace VehiculosReservasWebAPI.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public string Patente { get; set; } = null!;

    public int IdMarca { get; set; }

    public int? IdModelo { get; set; }

    public int IdTipo { get; set; }

    public int IdEstado { get; set; }

    public int Año { get; set; }

    public string? Color { get; set; }

    public int? Kilometraje { get; set; }

    public string? Transmision { get; set; }

    public string? Combustible { get; set; }

    public int? CapacidadPasajeros { get; set; }

    public decimal? PrecioCompra { get; set; }

    public string? Observaciones { get; set; }

    public DateOnly? FechaAlta { get; set; }

    public bool? Habilitado { get; set; }

    public virtual ICollection<Alquiler> Alquilers { get; set; } = new List<Alquiler>();

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual Modelo? IdModeloNavigation { get; set; }

    public virtual Tipo IdTipoNavigation { get; set; } = null!;

    public virtual ICollection<PrecioVehiculo> PrecioVehiculos { get; set; } = new List<PrecioVehiculo>();
}
