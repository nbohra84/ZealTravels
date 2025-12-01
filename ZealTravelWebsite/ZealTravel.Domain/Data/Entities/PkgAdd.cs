using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAdd
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Refid { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    public int? Noofnights { get; set; }

    public int? Starrating { get; set; }

    public int? StartingPrice { get; set; }

    public int? StartingPriceNonresident { get; set; }

    public string? Trip { get; set; }

    public DateOnly? ValidityTripStart { get; set; }

    public DateOnly? ValidityTripEnd { get; set; }

    public string? DepartureDates { get; set; }

    public string? PackageShowInHomePage { get; set; }

    public string? Themes { get; set; }

    public string? Inclusions { get; set; }

    public string? Exclusions { get; set; }

    public string? InstalmentPaymentOptions { get; set; }

    public DateTime? Eventtime { get; set; }
}
