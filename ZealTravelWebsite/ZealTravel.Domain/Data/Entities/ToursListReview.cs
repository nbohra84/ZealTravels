using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListReview
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }

    public DateTime? EventTime { get; set; }
}
