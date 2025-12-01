using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPaxSegmentDetailAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaxSegmentId { get; set; }

    public string? Conn { get; set; }

    public string? ChargeType { get; set; }

    public string? ChargeCode { get; set; }

    public string? ChargeDescription { get; set; }

    public decimal? ChargeAmount { get; set; }

    public DateTime? EventTime { get; set; }
}
