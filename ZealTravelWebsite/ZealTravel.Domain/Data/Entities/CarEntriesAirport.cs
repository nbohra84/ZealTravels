using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CarEntriesAirport
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public bool? Dropto { get; set; }

    public bool? Pickup { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? Tostation { get; set; }

    public DateOnly? PickupDate { get; set; }

    public DateTime? Eventtime { get; set; }
}
