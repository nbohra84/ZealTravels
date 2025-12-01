using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListExclusion
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Exclusions { get; set; }

    public DateTime? Eventtime { get; set; }
}
