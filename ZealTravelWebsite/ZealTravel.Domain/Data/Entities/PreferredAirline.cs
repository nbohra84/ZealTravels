using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PreferredAirline
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Airlinesname { get; set; }
}
