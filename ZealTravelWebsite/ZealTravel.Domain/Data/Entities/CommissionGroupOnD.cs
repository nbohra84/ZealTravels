using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CommissionGroupOnD
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public string? CarrierCode { get; set; }

    public decimal? Value { get; set; }

    public int? Sf { get; set; }

    public bool? PnrStatus { get; set; }
}
