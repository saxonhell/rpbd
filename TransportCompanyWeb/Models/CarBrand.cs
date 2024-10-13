using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class CarBrand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? TechnicalSpecifications { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
