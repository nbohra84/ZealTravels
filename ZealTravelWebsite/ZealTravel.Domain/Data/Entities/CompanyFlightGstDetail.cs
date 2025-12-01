using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyFlightGstDetail
{
    public int Id { get; set; }

    public int? BookingRef { get; set; }

    public string? GstcompanyContactNumber { get; set; }

    public string? GstcompanyName { get; set; }

    public string? Gstnumber { get; set; }

    public string? GstcompanyEmail { get; set; }

    public string? GstcompanyAddress { get; set; }

    public DateTime? EventTime { get; set; }
}
