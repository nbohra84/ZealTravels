using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgQuery
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public string? ContactPerson { get; set; }

    public string? Sector { get; set; }

    public string? Destination { get; set; }

    public string? DestinationDetail { get; set; }

    public int? Adt { get; set; }

    public int? Chd { get; set; }

    public int? Inf { get; set; }

    public DateOnly? CheckinDate { get; set; }

    public DateOnly? CheckoutDate { get; set; }

    public bool? Hotel { get; set; }

    public bool? Package { get; set; }

    public bool? SiteSeen { get; set; }

    public bool? Transferring { get; set; }

    public bool? Meals { get; set; }

    public bool? Flight { get; set; }

    public bool? Baggage { get; set; }

    public string? Source { get; set; }

    public DateOnly? SourceDepartureDate { get; set; }

    public DateOnly? SourceArrivalDate { get; set; }

    public bool? Passport { get; set; }

    public bool? Visa { get; set; }

    public bool? Cruise { get; set; }

    public bool? Insurance { get; set; }

    public string? Remark { get; set; }

    public string? Range { get; set; }

    public string? HotelCategory { get; set; }

    public string? StaffId { get; set; }

    public bool? Status { get; set; }

    public bool? IsRejected { get; set; }

    public string? RejectRemark { get; set; }

    public bool? IsConfirmed { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? EventTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public bool? DirectBilling { get; set; }

    public int? ReplyStatus { get; set; }
}
