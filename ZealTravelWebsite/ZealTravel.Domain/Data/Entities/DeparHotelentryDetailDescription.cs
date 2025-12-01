using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelentryDetailDescription
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int BookingRef { get; set; }

    public string? HotelDescription { get; set; }

    public string? HotelShortDescription { get; set; }

    public string? HotelType { get; set; }

    public string? HotelNearPoint { get; set; }

    public string? HotelEmail { get; set; }

    public string? HotelWebiste { get; set; }

    public string? HotelContact { get; set; }

    public string? HotelMobile { get; set; }

    public string? HotelPhone { get; set; }

    public string? Attractions { get; set; }

    public string? SpecialInstructions { get; set; }

    public decimal? Vat { get; set; }

    public string? Vattype { get; set; }

    public decimal? Commission { get; set; }

    public string? Commissiontype { get; set; }

    public string? Facilities { get; set; }

    public string? Amenity { get; set; }

    public string? Amenities { get; set; }

    public string? Services { get; set; }

    public string? Inclusion { get; set; }

    public string? PolicyTerms { get; set; }

    public string? CancellationPolicy { get; set; }

    public DateTime? Eventtime { get; set; }
}
