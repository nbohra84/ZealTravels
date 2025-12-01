using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyFlightOwnPnr
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? Pnr { get; set; }

    public string? FltType { get; set; }

    public DateTime EventTime { get; set; }
}
