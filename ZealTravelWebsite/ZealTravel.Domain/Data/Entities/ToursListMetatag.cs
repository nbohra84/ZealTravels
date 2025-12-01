using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListMetatag
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Title { get; set; }

    public string? Keywords { get; set; }

    public string? Descriptions { get; set; }

    public DateTime? Eventtime { get; set; }
}
