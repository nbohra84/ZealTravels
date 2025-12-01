using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CarEntry
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Refid { get; set; }

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public int? NoOfPassenger { get; set; }

    public bool? IsLocal { get; set; }

    public bool? IsOutstation { get; set; }

    public bool? IsAirport { get; set; }

    public string? Host { get; set; }

    public DateTime? Eventtime { get; set; }
}
