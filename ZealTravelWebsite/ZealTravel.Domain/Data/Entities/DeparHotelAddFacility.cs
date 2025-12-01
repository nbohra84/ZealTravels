using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddFacility
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Facilities { get; set; }

    public DateTime? Eventtime { get; set; }
}
