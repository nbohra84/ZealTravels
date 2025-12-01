using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddPolicy
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Checkin { get; set; }

    public string? Checkout { get; set; }

    public string? PaymentOptions { get; set; }

    public string? PolicyTerms { get; set; }

    public string? CancellationPolicy { get; set; }

    public DateTime? Eventtime { get; set; }
}
