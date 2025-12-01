using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelentryPassengerDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int BookingRef { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public DateTime? Eventtime { get; set; }
}
