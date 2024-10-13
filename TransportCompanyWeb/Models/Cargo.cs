using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class Cargo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CargoTypeId { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Features { get; set; }

    public virtual CargoType? CargoType { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
