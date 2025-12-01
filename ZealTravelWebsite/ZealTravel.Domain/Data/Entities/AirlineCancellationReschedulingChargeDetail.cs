using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineCancellationReschedulingChargeDetail
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public int? CancellationFee { get; set; }

    public int? ReBookingFee { get; set; }

    public string? Sector { get; set; }

    public string? CancellationRemark { get; set; }

    public string? ReBookingRemark { get; set; }
}
