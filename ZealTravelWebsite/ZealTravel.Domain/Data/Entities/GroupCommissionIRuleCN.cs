using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GroupCommissionIRuleCN
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? CarrierCode { get; set; }

    public decimal? Cb { get; set; }

    public int? Markup { get; set; }
}
