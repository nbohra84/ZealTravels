using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelRejectDetail
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? RejectDetail { get; set; }

    public DateTime? EventTime { get; set; }
}
