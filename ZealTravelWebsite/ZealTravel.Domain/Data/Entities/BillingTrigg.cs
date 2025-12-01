using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BillingTrigg
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? BookingType { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public bool? IsBilled { get; set; }
}
