using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class CarType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CargoType> CargoTypes { get; set; } = new List<CargoType>();

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
