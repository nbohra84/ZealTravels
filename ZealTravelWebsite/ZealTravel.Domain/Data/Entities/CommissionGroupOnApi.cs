using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CommissionGroupOnApi
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public string? GroupName { get; set; }

    public string? Sector { get; set; }

    public bool? RegistrationGroup { get; set; }

    public DateTime? EventTime { get; set; }
}
