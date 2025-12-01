using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class Logger
{
    public int Id { get; set; }

    public string? SearchCriteria { get; set; }

    public string? SearchId { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? MethodName { get; set; }

    public string? Location { get; set; }

    public string? ErrorMessage { get; set; }

    public string? Host { get; set; }

    public DateTime? EventTime { get; set; }
}
