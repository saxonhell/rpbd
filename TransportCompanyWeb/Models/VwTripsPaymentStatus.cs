using System;
using System.Collections.Generic;

namespace TransportCompanyWeb.Models;

public partial class VwTripsPaymentStatus
{
    public int TripId { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public string? Customer { get; set; }

    public decimal? Price { get; set; }

    public bool? PaymentStatus { get; set; }
}
