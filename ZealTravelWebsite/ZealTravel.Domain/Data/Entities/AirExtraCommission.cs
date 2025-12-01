using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirExtraCommission
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? Sector { get; set; }

    public string? CarrierCode { get; set; }

    public int? ExtraCommission { get; set; }

    public bool? Status { get; set; }
}
