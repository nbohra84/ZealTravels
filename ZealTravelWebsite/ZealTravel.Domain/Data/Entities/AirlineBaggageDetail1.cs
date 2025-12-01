using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineBaggageDetail1
{
    public int Id { get; set; }

    public string? Carrier { get; set; }

    public string? Sector { get; set; }

    public string? CheckIn { get; set; }

    public string? Cabin { get; set; }

    public string? Destination { get; set; }
}
