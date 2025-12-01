using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyMarkupIRule
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? CarrierCode { get; set; }

    public decimal? IValue { get; set; }

    public bool? Fixed { get; set; }

    public bool? Perce { get; set; }

    public bool? Basic { get; set; }

    public bool? Yq { get; set; }

    public bool? Total { get; set; }
}
