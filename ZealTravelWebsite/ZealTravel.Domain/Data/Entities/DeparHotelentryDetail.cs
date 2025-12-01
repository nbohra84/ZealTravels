using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelentryDetail
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? Sector { get; set; }

    public string? City { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

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

    public bool? IsUpdated { get; set; }

    public bool? IsRejected { get; set; }

    public string? StaffId { get; set; }

    public string? ConfirmationNo { get; set; }

    public int? TotalFare { get; set; }

    public decimal? Vat { get; set; }

    public decimal? Commission { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }
}
