using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelMarkup
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Markup { get; set; }

    public bool? IsFixed { get; set; }

    public bool? IsPercent { get; set; }

    public string? Sector { get; set; }
}
