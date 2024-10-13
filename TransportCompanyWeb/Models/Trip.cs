using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class Trip
{
    public int Id { get; set; }

    public int? CarId { get; set; }

    public string? Customer { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public DateOnly? DepartureDate { get; set; }

    public DateOnly? ArrivalDate { get; set; }

    public int? CargoId { get; set; }

    public decimal? Price { get; set; }

    public bool? PaymentStatus { get; set; }

    public bool? ReturnStatus { get; set; }

    public int? DriverId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Cargo? Cargo { get; set; }

    public virtual Employee? Driver { get; set; }
}
