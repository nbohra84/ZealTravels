using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyTransactionDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaxSegmentId { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public decimal? Balance { get; set; }

    public string? PaymentType { get; set; }

    public string? PaymentId { get; set; }

    public string? Remark { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? Status { get; set; }

    public string? EventId { get; set; }

    public bool? IsAirline { get; set; }

    public bool? IsHotel { get; set; }

    public bool? IsAirlineOff { get; set; }

    public bool? IsHotelOff { get; set; }

    public DateTime? EventTime { get; set; }
}
