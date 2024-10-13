using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class Car
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public int? CarTypeId { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public string? BodyNumber { get; set; }

    public string? EngineNumber { get; set; }

    public int? YearOfManufacture { get; set; }

    public int? DriverId { get; set; }

    public DateOnly? LastMaintenanceDate { get; set; }

    public int? MechanicId { get; set; }

    public virtual CarBrand? Brand { get; set; }

    public virtual CarType? CarType { get; set; }

    public virtual Employee? Driver { get; set; }

    public virtual Employee? Mechanic { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
