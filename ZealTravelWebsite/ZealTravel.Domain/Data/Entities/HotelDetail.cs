using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelDetail
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? Sector { get; set; }

    public string? Destination { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? InitialCheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public DateOnly? InitialCheckOutDate { get; set; }

    public DateTime? LastCancellationDate { get; set; }

    public int? Adt { get; set; }

    public int? Chd { get; set; }

    public int? NoOfRooms { get; set; }

    public string? HotelName { get; set; }

    public string? StarRating { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? CountryCode { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? City { get; set; }

    public string? SpecialRequest { get; set; }

    public bool? VoucherStatus { get; set; }

    public string? HotelBookingStatus { get; set; }

    public bool? IsUpdated { get; set; }

    public bool? IsRejected { get; set; }

    public string? MakerId { get; set; }

    public string? StaffId { get; set; }

    public bool? IsCancelRequested { get; set; }

    public bool? IsCanceled { get; set; }

    public bool? IsCanceledRejected { get; set; }

    public bool? IsRescheduled { get; set; }

    public string? ConfirmationNo { get; set; }

    public string? BookingId { get; set; }

    public bool? IsPriceChanged { get; set; }

    public bool? IsCancellationPolicyChanged { get; set; }

    public bool? IsUnderCancellationAllowed { get; set; }

    public bool? IsHotelPolicyChanged { get; set; }

    public string? InfoSource { get; set; }

    public int? InvoiceAmount { get; set; }

    public int? Markup { get; set; }

    public int? TotalFare { get; set; }

    public int? ServiceCharge { get; set; }

    public int? Commission { get; set; }

    public int? CommissionSa { get; set; }

    public int? Tds { get; set; }

    public int? TdsSa { get; set; }

    public string? InvoiceNo { get; set; }

    public string? HotelPolicyDetail { get; set; }

    public bool? Status { get; set; }

    public int? Totalcfee { get; set; }

    public DateTime? EventTime { get; set; }
}
