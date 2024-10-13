using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TransportCompanyWeb.Models;

namespace TransportCompanyWeb.Data;

public partial class TransportCompanyContext : DbContext
{
    public TransportCompanyContext()
    {
    }

    public TransportCompanyContext(DbContextOptions<TransportCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarBrand> CarBrands { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<CargoType> CargoTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<VwTransportInfo> VwTransportInfos { get; set; }

    public virtual DbSet<VwTripsPaymentStatus> VwTripsPaymentStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db7936.databaseasp.net; Database=db7936; User Id=db7936; Password=1234456789; Encrypt=False; MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC27ABABC2FF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BodyNumber).HasMaxLength(50);
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CarTypeId).HasColumnName("CarTypeID");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.EngineNumber).HasMaxLength(50);
            entity.Property(e => e.MechanicId).HasColumnName("MechanicID");
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.Cars)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Cars__BrandID__4316F928");

            entity.HasOne(d => d.CarType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarTypeId)
                .HasConstraintName("FK__Cars__CarTypeID__440B1D61");

            entity.HasOne(d => d.Driver).WithMany(p => p.CarDrivers)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Cars__DriverID__44FF419A");

            entity.HasOne(d => d.Mechanic).WithMany(p => p.CarMechanics)
                .HasForeignKey(d => d.MechanicId)
                .HasConstraintName("FK__Cars__MechanicID__45F365D3");
        });

        modelBuilder.Entity<CarBrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarBrand__3214EC27C8662829");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TechnicalSpecifications).HasMaxLength(255);
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarTypes__3214EC276BBE6A9B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cargos__3214EC2771C02D4E");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CargoTypeId).HasColumnName("CargoTypeID");
            entity.Property(e => e.Features).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CargoType).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.CargoTypeId)
                .HasConstraintName("FK__Cargos__CargoTyp__3E52440B");
        });

        modelBuilder.Entity<CargoType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CargoTyp__3214EC274776A29B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarTypeId).HasColumnName("CarTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CarType).WithMany(p => p.CargoTypes)
                .HasForeignKey(d => d.CarTypeId)
                .HasConstraintName("FK__CargoType__CarTy__3B75D760");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27C3485F6A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trips__3214EC2728849B9B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.CargoId).HasColumnName("CargoID");
            entity.Property(e => e.Customer).HasMaxLength(100);
            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.Origin).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Car).WithMany(p => p.Trips)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Trips__CarID__48CFD27E");

            entity.HasOne(d => d.Cargo).WithMany(p => p.Trips)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("FK__Trips__CargoID__49C3F6B7");

            entity.HasOne(d => d.Driver).WithMany(p => p.Trips)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Trips__DriverID__4AB81AF0");
        });

        modelBuilder.Entity<VwTransportInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TransportInfo");

            entity.Property(e => e.CargoName).HasMaxLength(100);
            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.DriverName).HasMaxLength(100);
            entity.Property(e => e.Origin).HasMaxLength(100);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.TripId).HasColumnName("TripID");
        });

        modelBuilder.Entity<VwTripsPaymentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TripsPaymentStatus");

            entity.Property(e => e.Customer).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.TripId).HasColumnName("TripID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
