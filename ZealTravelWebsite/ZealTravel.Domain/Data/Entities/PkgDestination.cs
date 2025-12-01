using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgDestination
{
    public int Id { get; set; }

    public string? Sector { get; set; }

    public string? Destination { get; set; }

    public string? DestinationDetail { get; set; }
}
