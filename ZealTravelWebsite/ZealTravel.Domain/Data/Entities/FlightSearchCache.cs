using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class FlightSearchCache
{
    public int CacheId { get; set; }

    public int Fidx { get; set; }

    public string Flight { get; set; } = null!;

    public string? Gds { get; set; }

    public DateTime Createdon { get; set; }

    public string? Segment { get; set; }

    public string? FareRule { get; set; }
}
