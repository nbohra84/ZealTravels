using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursExclusion
{
    public int Id { get; set; }

    public string? Exclusions { get; set; }
}
