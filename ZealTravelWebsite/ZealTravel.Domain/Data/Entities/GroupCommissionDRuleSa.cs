using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GroupCommissionDRuleSa
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? GroupId { get; set; }

    public string? CarrierCode { get; set; }

    public decimal? Commission { get; set; }
}
