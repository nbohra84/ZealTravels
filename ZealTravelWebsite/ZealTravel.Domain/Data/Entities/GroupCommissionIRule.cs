using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GroupCommissionIRule
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public int? Priority { get; set; }

    public string? SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? CarrierCode { get; set; }

    public decimal? Basic { get; set; }

    public decimal? BasicIata { get; set; }

    public decimal? Yq { get; set; }

    public decimal? YqIata { get; set; }

    public decimal? Cb { get; set; }

    public decimal? Promo { get; set; }

    public decimal? Sf { get; set; }

    public string? BookingClass { get; set; }

    public string? BookingClassNotValid { get; set; }

    public DateOnly? TripStartDate { get; set; }

    public DateOnly? TripEndDate { get; set; }

    public DateOnly? BookingStartDate { get; set; }

    public DateOnly? BookingEndDate { get; set; }

    public string? OriginCountry { get; set; }

    public string? DestinationCountry { get; set; }

    public bool? AutoPnr { get; set; }

    public bool? AutoTkt { get; set; }

    public bool? SplFare { get; set; }
}
