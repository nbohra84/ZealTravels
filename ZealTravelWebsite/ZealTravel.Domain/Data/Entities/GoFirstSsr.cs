using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GoFirstSsr
{
    public int Id { get; set; }

    public string? Sector { get; set; }

    public string? ProductClass { get; set; }

    public string? Ssrtype { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? Amount { get; set; }

    public bool? IsMoreThan10Hour { get; set; }
}
