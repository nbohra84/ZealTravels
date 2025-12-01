using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelSegmentDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? HotelSegmentId { get; set; }

    public bool? RequireAllPaxDetails { get; set; }

    public int? RoomStatus { get; set; }

    public string? RoomTypeCode { get; set; }

    public string? RoomTypeName { get; set; }

    public string? RatePlanCode { get; set; }

    public string? RatePlan { get; set; }

    public string? DayRates { get; set; }

    public string? SupplierPrice { get; set; }

    public string? RoomPromotion { get; set; }

    public string? Amenities { get; set; }

    public string? Amenity { get; set; }

    public string? SmokingPreference { get; set; }

    public string? HotelSupplements { get; set; }

    public string? CancellationPolicies { get; set; }

    public string? CancellationPolicy { get; set; }

    public DateTime? LastCancellationDate { get; set; }

    public string? Inclusion { get; set; }

    public decimal? RoomPrice { get; set; }

    public decimal? Tax { get; set; }

    public decimal? ExtraGuestCharge { get; set; }

    public decimal? ChildCharge { get; set; }

    public decimal? OtherCharges { get; set; }

    public decimal? Discount { get; set; }

    public int? Markup { get; set; }

    public int? TotalFare { get; set; }

    public int? ServiceCharge { get; set; }

    public decimal? PublishedPrice { get; set; }

    public int? PublishedPriceRoundedOff { get; set; }

    public decimal? OfferedPrice { get; set; }

    public int? OfferedPriceRoundedOff { get; set; }

    public decimal? AgentCommission { get; set; }

    public decimal? AgentMarkUp { get; set; }

    public decimal? ServiceTax { get; set; }

    public decimal? Tds { get; set; }

    public decimal? TotalGstamount { get; set; }
}
