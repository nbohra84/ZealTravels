using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyFlightDetailAirline
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? SupplierIdD { get; set; }
    public string? SupplierIdA { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? Sector { get; set; }

    public string? Trip { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? CarrierCodeD { get; set; }

    public string? CarrierCodeA { get; set; }

    public string? AirlinePnrD { get; set; }

    public string? AirlinePnrA { get; set; }

    public string? GdsPnrD { get; set; }

    public string? GdsPnrA { get; set; }

    public string? DepartureDateD { get; set; }

    public string? DepartureDateA { get; set; }

    public bool? IsImport { get; set; }

    public string? MakerId { get; set; }

    public string? StaffId { get; set; }

    public bool? IsOffline { get; set; }

    public bool? IsUpdated { get; set; }

    public bool? IsRejected { get; set; }

    public bool? IsCancelRequested { get; set; }

    public bool? IsCanceled { get; set; }

    public bool? IsCanceledRejected { get; set; }

    public bool? IsRescheduled { get; set; }

    public int? Adt { get; set; }

    public int? Chd { get; set; }

    public int? Inf { get; set; }

    public decimal? TotalTax { get; set; }

    public decimal? TotalBasic { get; set; }

    public decimal? TotalYq { get; set; }

    public decimal? TotalFare { get; set; }

    public decimal? TotalServiceTax { get; set; }

    public decimal? TotalMarkup { get; set; }

    public decimal? TotalBasicDeal { get; set; }

    public decimal? TotalYqDeal { get; set; }

    public decimal? TotalCbDeal { get; set; }

    public decimal? TotalPromoDeal { get; set; }

    public decimal? TotalServiceFeeDeal { get; set; }

    public decimal? TotalCommission { get; set; }

    public decimal? TotalTds { get; set; }

    public decimal? TotalSeat { get; set; }

    public decimal? TotalMeal { get; set; }

    public decimal? TotalBaggage { get; set; }

    public decimal? TotalImport { get; set; }

    public decimal? TotalMarkup1 { get; set; }

    public decimal? TotalBasicDeal1 { get; set; }

    public decimal? TotalYqDeal1 { get; set; }

    public decimal? TotalCbDeal1 { get; set; }

    public decimal? TotalPromoDeal1 { get; set; }

    public decimal? TotalCommission1 { get; set; }

    public decimal? TotalTds1 { get; set; }

    public decimal? TotalFare1 { get; set; }

    public decimal? Totalcfee { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public string? PriceTypeD { get; set; }

    public string? PriceTypeA { get; set; }

    public int? AgMarkupD { get; set; }

    public int? AgMarkupA { get; set; }

    public int? Fdmarkup { get; set; }

    public string? UniversalLocatorCodeD { get; set; }

    public string? UniversalLocatorCodeA { get; set; }
}
