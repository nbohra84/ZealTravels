using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPaxSegmentRescheduleDetailAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaxSegmentId { get; set; }

    public string? RescheduleType { get; set; }

    public string? Remark { get; set; }

    public string? FltType { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? DepartureDate { get; set; }

    public string? CarrierCode { get; set; }

    public string? FlightNumber { get; set; }

    public byte[]? Pnr { get; set; }

    public string? TiketNo { get; set; }

    public DateTime? EventTime { get; set; }
}
