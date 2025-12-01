using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListMap
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? AddressMap { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public DateTime? Eventtime { get; set; }
}
