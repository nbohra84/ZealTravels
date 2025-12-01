using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GroupCommissionPriceType
{
    public int Id { get; set; }

    public string? Supplierid { get; set; }

    public string? Sector { get; set; }

    public string? CarrierCode { get; set; }

    public string? PriceType { get; set; }

    public bool? Show { get; set; }

    public bool? Pnr { get; set; }

    public decimal? Basic { get; set; }

    public decimal? Yq { get; set; }

    public bool? Status { get; set; }

    public int? Sf { get; set; }

    public int? Cb { get; set; }
}
