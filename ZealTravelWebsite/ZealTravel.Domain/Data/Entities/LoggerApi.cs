using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LoggerApi
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? SearchId { get; set; }

    public string? SearchCriteria { get; set; }

    public string? Conn { get; set; }

    public string? MethodName { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public string? PassengerRequest { get; set; }

    public DateTime? EventTime { get; set; }
}
