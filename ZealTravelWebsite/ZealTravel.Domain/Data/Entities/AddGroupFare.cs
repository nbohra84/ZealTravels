using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AddGroupFare
{
    public int Id { get; set; }

    public string? Supplier { get; set; }

    public string? CompanyId { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? DepartureDate { get; set; }

    public string? DepartureTime { get; set; }

    public string? ArrivalTime { get; set; }

    public string? CarrierCode { get; set; }

    public string? FlightNumber { get; set; }

    public string? ClassOfService { get; set; }

    public string? Cabin { get; set; }

    public string? RuleTarrif { get; set; }

    public string? CabinBaggage { get; set; }

    public string? Baggage { get; set; }

    public int? Stops { get; set; }

    public int? AdultBasic { get; set; }

    public int? AdultYq { get; set; }

    public int? AdultTax { get; set; }

    public int? ChildBasic { get; set; }

    public int? ChildYq { get; set; }

    public int? ChildTax { get; set; }

    public int? InfantBasic { get; set; }

    public int? InfantTax { get; set; }

    public int? TotalSeat { get; set; }

    public int? SoldSeat { get; set; }

    public string? RefundType { get; set; }

    public string? PriceType { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }
}
