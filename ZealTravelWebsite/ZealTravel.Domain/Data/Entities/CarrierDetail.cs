using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CarrierDetail
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public string? CarrierName { get; set; }

    public bool? IsLcc { get; set; }

    public string? Iatacode { get; set; }

    public string? AirlineContact { get; set; }
}
