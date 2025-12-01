using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListPolicy
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Policy { get; set; }

    public string? Tnc { get; set; }

    public DateTime? Eventtime { get; set; }
}
