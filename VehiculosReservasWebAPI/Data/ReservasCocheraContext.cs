using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VehiculosReservasWebAPI.Models;

namespace VehiculosReservasWebAPI.Data;

public partial class ReservasCocheraContext : DbContext
{
    public ReservasCocheraContext()
    {
    }

    public ReservasCocheraContext(DbContextOptions<ReservasCocheraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alquiler> Alquilers { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Metodo> Metodos { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<OpcionAlquiler> OpcionAlquilers { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PrecioVehiculo> PrecioVehiculos { get; set; }

    public virtual DbSet<Tipo> Tipos { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-UV8F7GP; Database=ReservasCochera; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alquiler>(entity =>
        {
            entity.HasKey(e => e.IdAlquiler).HasName("PK__Alquiler__CB9A46B7D49209B5");

            entity.ToTable("Alquiler");

            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CostoRetraso)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 0)");
            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Finalizado).HasDefaultValue(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Alquilers)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alquiler_Cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Alquilers)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alquiler_Empleado");

            entity.HasOne(d => d.IdOpcionAlquilerNavigation).WithMany(p => p.Alquilers)
                .HasForeignKey(d => d.IdOpcionAlquiler)
                .HasConstraintName("FK_Alquiler_OpcionAlquiler");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Alquilers)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alquiler_Vehiculo");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D594664238EB2DC7");

            entity.ToTable("Cliente");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Habilitado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E3EEF39EC");

            entity.ToTable("Empleado");

            entity.Property(e => e.Clave)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Habilitado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__FBB0EDC17FA57395");

            entity.ToTable("Estado");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__4076A887D106C744");

            entity.ToTable("Marca");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Metodo>(entity =>
        {
            entity.HasKey(e => e.IdMetodo).HasName("PK__Metodo__63A212898B1FBD94");

            entity.ToTable("Metodo");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PK__Modelo__CC30D30C499EC1A3");

            entity.ToTable("Modelo");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Modelo__IdMarca__5EBF139D");
        });

        modelBuilder.Entity<OpcionAlquiler>(entity =>
        {
            entity.HasKey(e => e.IdOpcionAlquiler).HasName("PK__OpcionAl__F321395E3B3586D9");

            entity.ToTable("OpcionAlquiler");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__FC851A3A2F67F99C");

            entity.ToTable("Pago");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Importe).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Empleado");

            entity.HasOne(d => d.IdMetodoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdMetodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Metodo");
        });

        modelBuilder.Entity<PrecioVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdPrecioVehiculo).HasName("PK__PrecioVe__3D4FDF55001660C7");

            entity.ToTable("PrecioVehiculo");

            entity.Property(e => e.PrecioPorDia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioPorHora).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.PrecioVehiculos)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecioVehiculo_Vehiculo");
        });

        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PK__Tipo__9E3A29A52A62A033");

            entity.ToTable("Tipo");

            entity.Property(e => e.CostoAlquiler).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DescripcionAmpliada)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__7086121507C8F940");

            entity.ToTable("Vehiculo");

            entity.HasIndex(e => e.Patente, "UQ__Vehiculo__CA6551661F0E20CC").IsUnique();

            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Combustible)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaAlta).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Patente)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Transmision)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehiculo__IdEsta__66603565");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehiculo__IdMarc__6383C8BA");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdModelo)
                .HasConstraintName("FK__Vehiculo__IdMode__6477ECF3");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehiculo__IdTipo__656C112C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
