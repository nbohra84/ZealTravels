using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursInclusion
{
    public int Id { get; set; }

    public string? Inclusions { get; set; }
}
