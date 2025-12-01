using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddMetum
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaKeywords { get; set; }

    public string? MetaDescription { get; set; }

    public DateTime? Eventtime { get; set; }
}
