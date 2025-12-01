using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPaxSegmentCancellationDetailAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? CPaxSegmentId { get; set; }

    public int? PaxSegmentId { get; set; }

    public string? CanceledBy { get; set; }

    public string? CancellationType { get; set; }

    public string? CancellationReason { get; set; }

    public string? Remark { get; set; }

    public string? FltType { get; set; }

    public bool? IsPartial { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? Pnr { get; set; }

    public string? TicketNo { get; set; }

    public string? CarrierCode { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public string? Ip { get; set; }
}
