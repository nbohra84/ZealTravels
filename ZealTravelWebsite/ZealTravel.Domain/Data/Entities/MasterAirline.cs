using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class MasterAirline
{
    public string? CarrierCode { get; set; }

    public string? PrefixCode { get; set; }

    public string? AirlineName { get; set; }
}
