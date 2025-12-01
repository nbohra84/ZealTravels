using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LogCompanyFlightSegmentDetailAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? FlightSegmentId { get; set; }

    public string? CarrierCode { get; set; }

    public string? AirlinePnr { get; set; }

    public string? GdsPnr { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? DepartureStation { get; set; }

    public string? ArrivalStation { get; set; }

    public string? DepartureDate { get; set; }

    public string? ArrivalDate { get; set; }

    public string? DepartureTime { get; set; }

    public string? ArrivalTime { get; set; }

    public string? FlightNumber { get; set; }

    public string? ClassOfService { get; set; }

    public int? Stops { get; set; }

    public int? Via { get; set; }

    public string? ViaName { get; set; }

    public string? DepartureTerminal { get; set; }

    public string? ArrivalTerminal { get; set; }

    public int? JourneyTime { get; set; }

    public int? Duration { get; set; }

    public string? ProductClass { get; set; }

    public string? FareBasisCode { get; set; }

    public string? Cabin { get; set; }

    public string? RefundType { get; set; }

    public string? FareType { get; set; }

    public string? ConnOrder { get; set; }

    public DateTime? EventTime { get; set; }

    public string? UpdateType { get; set; }

    public DateTime? UpdateTime { get; set; }
}
