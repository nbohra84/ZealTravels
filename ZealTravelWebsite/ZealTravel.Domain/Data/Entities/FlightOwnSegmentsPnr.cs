using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class FlightOwnSegmentsPnr
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? PnrGalileo { get; set; }

    public string? PnrAirline { get; set; }

    public string? FltType { get; set; }

    public DateTime? EventTime { get; set; }
}
