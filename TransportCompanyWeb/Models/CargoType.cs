using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class CargoType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CarTypeId { get; set; }

    public string? Description { get; set; }

    public virtual CarType? CarType { get; set; }

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
}
