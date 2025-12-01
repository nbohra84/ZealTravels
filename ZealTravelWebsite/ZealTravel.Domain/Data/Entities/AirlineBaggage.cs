using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineBaggage
{
    public int Id { get; set; }

    public string? GdsCode { get; set; }

    public string? SourceCountry { get; set; }

    public string? DestinationCountry { get; set; }

    public string? Destination { get; set; }

    public string? CabinBaggage { get; set; }

    public string? CheckInBaggage { get; set; }

    public bool? IsHandBaggage { get; set; }
}
