using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DepartureFaresPassengerBook
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Age { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public DateTime? EventTime { get; set; }
}
