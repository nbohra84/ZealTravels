using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BookingsMailSmsDetail
{
    public int Id { get; set; }

    public int BookingRef { get; set; }

    public string? BookingType { get; set; }

    public bool? Mail { get; set; }

    public bool? Sms { get; set; }

    public DateTime? EventTime { get; set; }
}
