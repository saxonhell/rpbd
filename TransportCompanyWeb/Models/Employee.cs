using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Car> CarDrivers { get; set; } = new List<Car>();

    public virtual ICollection<Car> CarMechanics { get; set; } = new List<Car>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
