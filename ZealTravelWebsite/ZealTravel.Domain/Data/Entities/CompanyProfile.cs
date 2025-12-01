using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyProfile
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Notification { get; set; }

    public bool? Status { get; set; }
}
