using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CustomerRefund
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? Passengerid { get; set; }

    public decimal? RefundAmount { get; set; }

    public int? Pgcharges { get; set; }

    public bool? RefundStatus { get; set; }

    public DateTime? RefundEventTime { get; set; }

    public DateTime? EventTime { get; set; }
}
