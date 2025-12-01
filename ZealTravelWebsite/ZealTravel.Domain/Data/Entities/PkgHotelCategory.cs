using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgHotelCategory
{
    public int Id { get; set; }

    public string? Sector { get; set; }

    public int? Star { get; set; }
}
