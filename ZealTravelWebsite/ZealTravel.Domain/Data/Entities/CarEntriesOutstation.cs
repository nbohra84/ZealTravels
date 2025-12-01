using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CarEntriesOutstation
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Trip { get; set; }

    public string? CityFrom { get; set; }

    public string? CityTo { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public DateTime? Eventtime { get; set; }
}
