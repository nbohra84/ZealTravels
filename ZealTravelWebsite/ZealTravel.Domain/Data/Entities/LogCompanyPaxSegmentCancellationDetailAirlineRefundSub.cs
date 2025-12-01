using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LogCompanyPaxSegmentCancellationDetailAirlineRefundSub
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? StaffId { get; set; }

    public int? BookingRef { get; set; }

    public int CPaxSegmentId { get; set; }

    public int? PaxSegmentId { get; set; }

    public string? Reply { get; set; }

    public string? RefundType { get; set; }

    public decimal? Basic { get; set; }

    public decimal? Yq { get; set; }

    public decimal? Gst { get; set; }

    public decimal? Taxes { get; set; }

    public decimal? Markup { get; set; }

    public decimal? ServiceFee { get; set; }

    public decimal? Baggage { get; set; }

    public decimal? Meal { get; set; }

    public decimal? Seat { get; set; }

    public decimal? Commission { get; set; }

    public decimal? Tds { get; set; }

    public decimal? OurCharges { get; set; }

    public decimal? CompanyCancelCharges { get; set; }

    public decimal? AirlineCharges { get; set; }

    public decimal? NetRefundAmount { get; set; }

    public DateTime? EventTime { get; set; }

    public int? VerifyStatus { get; set; }

    public string? VerifyStaffId { get; set; }

    public DateTime? VerifyEventTime { get; set; }

    public string? PayStaffId { get; set; }

    public DateTime? PayEventTime { get; set; }

    public string? UpdateType { get; set; }

    public DateTime? UpdateTime { get; set; }
}
