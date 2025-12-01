using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HolidaysQuery
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? ContactPerson { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Destination { get; set; }

    public string? Remark { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }
}
