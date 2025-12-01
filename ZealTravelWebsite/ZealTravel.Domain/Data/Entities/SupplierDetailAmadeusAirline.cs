using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierDetailAmadeusAirline
{
    public int Id { get; set; }

    public string? Corporateid { get; set; }

    public string? Currency { get; set; }

    public string? OfficeId { get; set; }

    public string? Originator { get; set; }

    public string? Password { get; set; }

    public int? PwdLength { get; set; }

    public string? SellCity { get; set; }

    public string? Url { get; set; }

    public string? Wsap { get; set; }
}
