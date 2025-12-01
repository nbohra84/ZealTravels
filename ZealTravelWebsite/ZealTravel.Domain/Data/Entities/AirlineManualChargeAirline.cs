using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineManualChargeAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Description { get; set; }

    public int? ChargeAmount { get; set; }

    public string? Sector { get; set; }
}
