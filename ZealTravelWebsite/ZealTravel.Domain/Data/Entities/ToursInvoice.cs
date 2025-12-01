using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursInvoice
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? Tourid { get; set; }

    public string? ShortOverview { get; set; }

    public string? ShortDescription { get; set; }

    public int? Rating { get; set; }

    public string? TourName { get; set; }

    public string? TourCountry { get; set; }

    public string? TourCurrency { get; set; }

    public int? AdultsQuantity { get; set; }

    public int? ChildsQuantity { get; set; }

    public int? InfantsQuantity { get; set; }

    public int? AdultsPrice { get; set; }

    public int? ChildsPrice { get; set; }

    public int? InfantsPrice { get; set; }

    public int? TourStar { get; set; }

    public int? TourDays { get; set; }

    public int? TourNightDays { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public DateTime? EventTime { get; set; }
}
