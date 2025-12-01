using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirOffline
{
    public int Id { get; set; }

    public string? Supplier { get; set; }

    public string? CompanyId { get; set; }

    public string? UpdateBy { get; set; }

    public int? BookingRef { get; set; }

    public string? Trip { get; set; }

    public string? Sector { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public int? Adt { get; set; }

    public int? Chd { get; set; }

    public int? Inf { get; set; }

    public string? DepartureAirportOutbound { get; set; }

    public string? DepartureStationOutbound { get; set; }

    public string? ArrivalAirportOutbound { get; set; }

    public string? ArrivalStationOutbound { get; set; }

    public string? DepDateOutbound { get; set; }

    public string? ArrDateOutbound { get; set; }

    public string? DepTimeOutbound { get; set; }

    public string? ArrTimeOutbound { get; set; }

    public string? CarrierCodeOutbound { get; set; }

    public string? FlightNumberOutbound { get; set; }

    public string? ClassOutbound { get; set; }

    public string? StopsOutbound { get; set; }

    public string? DepartureAirportInbound { get; set; }

    public string? DepartureStationInbound { get; set; }

    public string? ArrivalAirportInbound { get; set; }

    public string? ArrivalStationInbound { get; set; }

    public string? DepDateInbound { get; set; }

    public string? ArrDateInbound { get; set; }

    public string? DepTimeInbound { get; set; }

    public string? ArrTimeInbound { get; set; }

    public string? CarrierCodeInbound { get; set; }

    public string? FlightNumberInbound { get; set; }

    public string? ClassInbound { get; set; }

    public string? StopsInbound { get; set; }

    public string? PaxType { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Dob { get; set; }

    public string? PassportNumber { get; set; }

    public string? TicketNumber { get; set; }

    public string? PnrOutbound { get; set; }

    public string? PnrInbound { get; set; }

    public string? Email { get; set; }

    public string? MobileNo { get; set; }

    public int? AdultBasicOutbound { get; set; }

    public int? AdultYqOutbound { get; set; }

    public int? AdultTaxOutbound { get; set; }

    public int? AdultTotalOutbound { get; set; }

    public int? ChildBasicOutbound { get; set; }

    public int? ChildYqOutbound { get; set; }

    public int? ChildTaxOutbound { get; set; }

    public int? ChildTotalOutbound { get; set; }

    public int? InfantBasicOutbound { get; set; }

    public int? InfantTaxOutbound { get; set; }

    public int? InfantTotalOutbound { get; set; }

    public int? AdultBasicInbound { get; set; }

    public int? AdultYqInbound { get; set; }

    public int? AdultTaxInbound { get; set; }

    public int? AdultTotalInbound { get; set; }

    public int? ChildBasicInbound { get; set; }

    public int? ChildYqInbound { get; set; }

    public int? ChildTaxInbound { get; set; }

    public int? ChildTotalInbound { get; set; }

    public int? InfantBasicInbound { get; set; }

    public int? InfantTaxInbound { get; set; }

    public int? InfantTotalInbound { get; set; }

    public int? TotalFare { get; set; }

    public string? BookingStatus { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public bool? IsBilled { get; set; }
}
