using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class StaffCategory
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? StaffType { get; set; }

    public bool? IsB2b { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? IsCorporate { get; set; }
}
