using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CarEntriesLocal
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public DateOnly? PickupDate { get; set; }

    public string? PickupTime { get; set; }

    public string? DropoffTime { get; set; }

    public DateTime? Eventtime { get; set; }
}
