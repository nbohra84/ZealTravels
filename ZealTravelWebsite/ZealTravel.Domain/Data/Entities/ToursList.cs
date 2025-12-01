using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursList
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Tourid { get; set; }

    public int? Tourorder { get; set; }

    public string? ShortOverview { get; set; }

    public string? ShortDescription { get; set; }

    public int? Rating { get; set; }

    public string? TourName { get; set; }

    public string? TourCountry { get; set; }

    public string? TourDescription { get; set; }

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

    public string? TourType { get; set; }

    public bool? Featured { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public bool? Status { get; set; }

    public DateTime? Eventtime { get; set; }
}
