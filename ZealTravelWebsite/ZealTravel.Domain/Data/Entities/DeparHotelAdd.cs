using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAdd
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Refid { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? HotelName { get; set; }

    public string? HotelDescription { get; set; }

    public string? HotelShortDescription { get; set; }

    public string? HotelType { get; set; }

    public int? Stars { get; set; }

    public string? Location { get; set; }

    public string? HotelAddress { get; set; }

    public string? HotelNearPoint { get; set; }

    public string? HotelEmail { get; set; }

    public string? HotelWebiste { get; set; }

    public string? HotelContact { get; set; }

    public string? HotelMobile { get; set; }

    public string? HotelPhone { get; set; }

    public string? Attractions { get; set; }

    public string? SpecialInstructions { get; set; }

    public string? AddressOnMap { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public bool? Status { get; set; }

    public decimal? Vat { get; set; }

    public string? Vattype { get; set; }

    public decimal? Commission { get; set; }

    public string? Commissiontype { get; set; }

    public DateTime? Eventtime { get; set; }
}
