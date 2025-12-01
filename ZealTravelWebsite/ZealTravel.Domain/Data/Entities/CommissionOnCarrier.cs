using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CommissionOnCarrier
{
    public int Id { get; set; }

    public string CarrierCode { get; set; } = null!;

    public bool? IsDomestic { get; set; }
}
