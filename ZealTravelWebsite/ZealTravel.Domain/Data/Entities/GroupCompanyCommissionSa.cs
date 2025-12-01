using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GroupCompanyCommissionSa
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Sector { get; set; }

    public int? GroupId { get; set; }
}
