using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterProductAdded
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public bool? AirDepartures { get; set; }

    public bool? HotelDepartures { get; set; }

    public bool? Tours { get; set; }
}
