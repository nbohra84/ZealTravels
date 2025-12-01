using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListContact
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public DateTime? Eventtime { get; set; }
}
