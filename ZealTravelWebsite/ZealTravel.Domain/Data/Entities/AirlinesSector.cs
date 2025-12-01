using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlinesSector
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public string? DepartureStation { get; set; }

    public string? ArrivalStation { get; set; }
}
