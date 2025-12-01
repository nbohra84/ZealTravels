using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPaxSegmentCancellationDetailAirlineRemark
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? Remark { get; set; }

    public string? StaffId { get; set; }

    public DateTime? EventTime { get; set; }
}
