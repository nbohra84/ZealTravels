using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlinePnrMakeDay
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public string? Sector { get; set; }

    public int? PnrDays { get; set; }
}
