using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class VwTransportInfo
{
    public int TripId { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public string CargoName { get; set; } = null!;

    public string DriverName { get; set; } = null!;

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public DateOnly? DepartureDate { get; set; }

    public DateOnly? ArrivalDate { get; set; }
}
