using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCallCenter
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int BookingRef { get; set; }

    public string? StaffId { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }
}
