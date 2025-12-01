using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyFareDetailAirline
{
    public int Id { get; set; }
    public string? CompanyId { get; set; }
    public int? BookingRef { get; set; }
    public string? PriceType { get; set; }
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
    public decimal? TotalQueue { get; set; }
    public string? Conn { get; set; }
    public DateTime? EventTime { get; set; }
    public bool? IsPaymentHold { get; set; }
}

