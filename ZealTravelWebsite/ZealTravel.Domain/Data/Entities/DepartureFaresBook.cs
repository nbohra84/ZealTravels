using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DepartureFaresBook
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? Sector { get; set; }

    public string? CompanyId { get; set; }

    public int? RefId { get; set; }

    public int? BookingRef { get; set; }

    public string? DepartureStation { get; set; }

    public string? ArrivalStation { get; set; }

    public string? DepartureCarrierCode { get; set; }

    public int? DepartureFlightNumber { get; set; }

    public string? DepartureDate { get; set; }

    public string? DepartureTime { get; set; }

    public string? DepartureArrivalDate { get; set; }

    public string? DepartureArrivalTime { get; set; }

    public int? DepartureStops { get; set; }

    public string? ReturnCarrierCode { get; set; }

    public int? ReturnFlightNumber { get; set; }

    public string? ReturnDate { get; set; }

    public string? ReturnTime { get; set; }

    public string? ReturnArrivalDate { get; set; }

    public string? ReturnArrivalTime { get; set; }

    public int? ReturnStops { get; set; }

    public int? Markup { get; set; }

    public int? Gst { get; set; }

    public int? Fare { get; set; }

    public int? TotalFare { get; set; }

    public int? NoOfPassengers { get; set; }

    public string? Pnr { get; set; }

    public bool? Reject { get; set; }

    public string? UpdateBy { get; set; }

    public string? DepartureTerminal { get; set; }

    public string? ArrivalTerminal { get; set; }

    public DateTime? EventTime { get; set; }
}
