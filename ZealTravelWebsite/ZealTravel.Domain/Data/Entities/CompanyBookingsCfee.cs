using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyBookingsCfee
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? BookingType { get; set; }

    public int? CfeeSa { get; set; }

    public int? CfeeCu { get; set; }

    public bool? Iswallet { get; set; }

    public string? Sector { get; set; }
}
